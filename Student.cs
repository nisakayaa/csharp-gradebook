namespace Gradebook;

public class Student
{
    public string Id { get; set; } = "";
    public string FullName { get; set; } = "";
    public List<int> Grades { get; set; } = new();

    public double Average => Grades.Count == 0 ? 0 : Grades.Average();
}
