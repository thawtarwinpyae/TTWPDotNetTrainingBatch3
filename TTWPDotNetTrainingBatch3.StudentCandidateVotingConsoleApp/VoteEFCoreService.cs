using StudentCandidateConsoleApp.Database.AppDbContext.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTWPDotNetTrainingBatch3.StudentCandidateVotingConsoleApp
{
    public class VoteEFCoreService
    {
        public void Read()
        {
            AppDbContext db = new AppDbContext();
            Console.WriteLine("=== Voting Results ===");
            var results = db.Candidates
                .Select(c => new { c.Name, c.VoteCount })
                .OrderByDescending(c => c.VoteCount)
                .ToList();

            foreach (var result in results)
            {
                Console.WriteLine($"{result.Name}: {result.VoteCount} votes");
            }
        }
        public void Create()
        {
            AppDbContext db = new AppDbContext();
            int studentId;
            int candidateId;

            while (true)
            {
                Console.Write("Enter your Student ID: ");
                if (int.TryParse(Console.ReadLine(), out studentId))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Enter a valid ID!");
                }
            }
               
            var student = db.Students.FirstOrDefault(s => s.Id == studentId);

            if (student == null)
            {
                Console.WriteLine("Student not found.");
                return;
            }

            if ((bool)student.HasVoted)
            {
                Console.WriteLine("You have already voted.");
                return;
            }

            Console.WriteLine("Candidates:");
            var candidates = db.Candidates.ToList();
            foreach (var c in candidates)
            {
                Console.WriteLine($"{c.Id}. {c.Name}");
            }


            while (true)
            {
                Console.Write("Enter Candidate ID to vote for: ");

                if (int.TryParse(Console.ReadLine(), out candidateId)) 
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Enter a valid ID!");

                }
            }
            
            var candidate = db.Candidates.FirstOrDefault(c => c.Id == candidateId);
            if (candidate == null)
            {
                Console.WriteLine("Candidate not found.");
                return;
            }

            db.Votes.Add(new Vote
            {
                StudentId = studentId,
                CandidateId = candidateId,
                VotedAt = DateTime.Now
            });

            candidate.VoteCount += 1;
            student.HasVoted = true;
            db.SaveChanges();

            Console.WriteLine($"Vote cast for {candidate.Name}.");
        }
    }
}
