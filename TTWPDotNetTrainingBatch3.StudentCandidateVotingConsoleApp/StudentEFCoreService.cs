using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using StudentCandidateConsoleApp.Database.AppDbContext.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTWPDotNetTrainingBatch3.StudentCandidateVotingConsoleApp
{
    public class StudentEFCoreService
    {
        public void Read()
        {
            AppDbContext db = new AppDbContext();

            var students = db.Students.ToList();
            foreach (var s in students)
            {
                Console.WriteLine($"{s.Id}. {s.Name} - Voted: {s.HasVoted}");
            }


        }
        public void Create()
        {
            AppDbContext db = new AppDbContext();

            Console.Write("Enter student name: ");
            var name = Console.ReadLine();

            var student = new Student()
            {
                Name = name,
                HasVoted=false,
                DeleteFlag=false,
                CreatedDateTime = DateTime.Now
            };

            db.Students.Add(student);
            int result = db.SaveChanges();
            string message = result>0 ? "Student added." : "No Student Added!";
            Console.WriteLine(message);

        }
        public void Update()
        {
            AppDbContext db = new AppDbContext();

            int id;

            while(true)
            {
                Console.Write("Enter student ID to update: ");
                string input = Console.ReadLine();

                if (int.TryParse(input, out id))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Not an Id Number! Please enter a correct ID!");
                }

            }
               

            var student = db.Students.Find(id);
            if (student == null)
            { 
                Console.WriteLine("Not found!"); 
                return;
            }

            Console.Write("Enter new name: ");
            student.Name = Console.ReadLine();
            student.ModifiedDateTime = DateTime.Now;

            int result = db.SaveChanges();
            string message = result>0 ? "Student updated." : "No Student Updated!";
            Console.WriteLine(message);
         

        }
        public void Delete()
        {
            AppDbContext db = new AppDbContext();

            Console.Write("Enter student ID to delete: ");
            int id = int.Parse(Console.ReadLine());

            var student = db.Students.Where(x => x.Id == id).FirstOrDefault();

            if (student is null)
            {
                Console.WriteLine("Not found.");
                return;
            }

            student.DeleteFlag = true;
            student.ModifiedDateTime=DateTime.Now;

            int result = db.SaveChanges();
            string message = result>0 ? "Student deleted!" : "No Student Deleted!";
            Console.WriteLine(message);
        }

       // dotnet ef dbcontext scaffold "Server=.;Database=StudentCandidateVoting;User ID=sa;Password=sasa@123;TrustServerCertificate=true;" Microsoft.EntityFrameworkCore.SqlServer -o AppDbContext.Models -c AppDbContext -f
             
    }
}
