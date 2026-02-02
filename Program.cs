using System.Text.Json;
using Gradebook;

const string DbFile = "gradebook.json";
var options = new JsonSerializerOptions { WriteIndented = true };

List<Student> students = Load();

while (true)
{
    Console.WriteLine("\n1) Add student  2) Add grade  3) List  4) Top  5) Save & Exit");
    Console.Write("Select: ");
    var input = Console.ReadLine()?.Trim();

    if (input == "1") AddStudent();
    else if (input == "2") AddGrade();
    else if (input == "3") ListStudents();
    else if (input == "4") ShowTop();
    else if (input == "5") { Save(); break; }
    else Console.WriteLine("Invalid option.");
}

List<Student> Load()
{
    if (!File.Exists(DbFile)) return new List<Student>();
    var json = File.ReadAllText(DbFile);
    return JsonSerializer.Deserialize<List<Student>>(json) ?? new List<Student>();
}

void Save()
{
    var json = JsonSerializer.Serialize(students, options);
    File.WriteAllText(DbFile, json);
    Console.WriteLine("Saved to gradebook.json");
}

void AddStudent()
{
    Console.Write("Student ID: ");
    var id = Console.ReadLine()?.Trim() ?? "";
    if (id.Length < 2 || students.Any(s => s.Id == id))
    {
        Console.WriteLine("Invalid or duplicate ID.");
        return;
    }

    Console.Write("Full name: ");
    var name = Console.ReadLine()?.Trim() ?? "";
    if (name.Length < 3)
    {
        Console.WriteLine("Name too short.");
        return;
    }

    students.Add(new Student { Id = id, FullName = name });
    Console.WriteLine("Added.");
}

void AddGrade()
{
    Console.Write("Student ID: ");
    var id = Console.ReadLine()?.Trim() ?? "";
    var s = students.FirstOrDefault(x => x.Id == id);
    if (s == null) { Console.WriteLine("Not found."); return; }

    Console.Write("Grade (0-100): ");
    if (!int.TryParse(Console.ReadLine(), out var g) || g < 0 || g > 100)
    {
        Console.WriteLine("Invalid grade.");
        return;
    }

    s.Grades.Add(g);
    Console.WriteLine("Grade added.");
}

void ListStudents()
{
    if (students.Count == 0) { Console.WriteLine("No students."); return; }

    foreach (var s in students.OrderByDescending(x => x.Average))
        Console.WriteLine($"{s.Id} | {s.FullName} | Avg: {s.Average:F2} | Grades: {string.Join(",", s.Grades)}");
}

void ShowTop()
{
    var top = students.OrderByDescending(s => s.Average).FirstOrDefault();
    if (top == null) { Console.WriteLine("No data."); return; }
    Console.WriteLine($"Top: {top.FullName} ({top.Id}) Avg={top.Average:F2}");
}
