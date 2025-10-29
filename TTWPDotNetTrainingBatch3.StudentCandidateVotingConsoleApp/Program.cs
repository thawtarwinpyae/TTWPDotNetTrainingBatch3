// See https://aka.ms/new-console-template for more information
using TTWPDotNetTrainingBatch3.StudentCandidateVotingConsoleApp;

//Console.WriteLine("Hello, World!");

VoteEFCoreService vote = new VoteEFCoreService();

//student.Read();
//student.Create();
//student.Update();
//student.Delete();

//candidate.Read();
//candidate.Create();
//candidate.Update();
//candidate.Delete();

//vote.Create();
//vote.Read();

//ManageStudents();
//ManageCandidates();

Console.WriteLine("=== Welcome to Student Candidate Voting System ===");
Console.WriteLine("1. Manage Students");
Console.WriteLine("2. Manage Candidates");
Console.WriteLine("3. Vote for Candidate");
Console.WriteLine("4. View Voting Result");
Console.Write("Choose one of the above options: ");


var voteChoice = Console.ReadLine();

switch (voteChoice)
{
    case "1": ManageStudents(); break;
    case "2": ManageCandidates(); break;
    case "3":vote.Create();break;
    case "4":vote.Read();break;
}


static void ManageStudents() 
{
    StudentEFCoreService student = new StudentEFCoreService();

    Console.Clear();
    Console.WriteLine("=== Manage Students ===");
    Console.WriteLine("1. Add Student");
    Console.WriteLine("2. View Students");
    Console.WriteLine("3. Update Student");
    Console.WriteLine("4. Delete Student");
    Console.Write("Choose an option from 1 to 4: ");
    var choice = Console.ReadLine();

    switch (choice)
    {
        case "1": student.Create(); break;
        case "2": student.Read(); break;
        case "3": student.Update(); break;
        case "4": student.Delete(); break;
        default: Console.WriteLine("Invalid choice."); break;
    }
}

static void ManageCandidates()
{
    CandidateEFCoreService candidate = new CandidateEFCoreService();

    Console.Clear();
    Console.WriteLine("=== Manage Candidates ===");
    Console.WriteLine("1. Add Candidate");
    Console.WriteLine("2. View Candidates");
    Console.WriteLine("3. Update Candidate");
    Console.WriteLine("4. Delete Candidate");
    Console.Write("Choose an option: ");
    var choice = Console.ReadLine();

    switch (choice)
    {
        case "1": candidate.Create(); break;
        case "2": candidate.Read(); break;
        case "3": candidate.Update(); break;
        case "4": candidate.Delete(); break;
        default: Console.WriteLine("Invalid choice."); break;
    }
}

