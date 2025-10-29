using StudentCandidateConsoleApp.Database.AppDbContext.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTWPDotNetTrainingBatch3.StudentCandidateVotingConsoleApp
{
    public class CandidateEFCoreService
    {
        public void Read()
        {
            AppDbContext db = new AppDbContext();

            var candidates = db.Candidates.ToList();
            foreach (var c in candidates)
            {
                Console.WriteLine($"{c.Id}. {c.Name} - Votes: {c.VoteCount}");
            }
        }
        public void Create()
        {
            AppDbContext db = new AppDbContext();

            Console.Write("Enter candidate name: ");
            var name = Console.ReadLine();

            db.Candidates.Add(new Candidate { Name = name, VoteCount = 0, DeleteFlag=false,CreatedDateTime=DateTime.Now });
            int result= db.SaveChanges();

            string message = result>0 ? "Candidate added." : "No Candidate Added!";
            Console.WriteLine(message);
        }

        public void Update()
        {
            AppDbContext db = new AppDbContext();

            int id;

            while (true)
            {
                Console.Write("Enter candidate ID to update: ");
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


            var candidate = db.Candidates.Find(id);
            if (candidate == null)
            {
                Console.WriteLine("Not found!");
                return;
            }

            Console.Write("Enter new name: ");
            candidate.Name = Console.ReadLine();
            candidate.ModifiedDateTime = DateTime.Now;

            int result = db.SaveChanges();
            string message = result>0 ? "Candidate updated." : "No Candidate Updated!";
            Console.WriteLine(message);
        }

        public void Delete()
        {
            AppDbContext db = new AppDbContext();

            Console.Write("Enter candidate ID to delete: ");
            int id = int.Parse(Console.ReadLine());

            var candidate = db.Candidates.Where(x => x.Id == id).FirstOrDefault();

            if (candidate is null)
            {
                Console.WriteLine("Not found.");
                return;
            }

            candidate.DeleteFlag = true;
            candidate.ModifiedDateTime=DateTime.Now;

            int result = db.SaveChanges();
            string message = result>0 ? "Candidate deleted!" : "No Candidate Deleted!";
            Console.WriteLine(message);
        }
    }
}
