using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTWPDotNetTrainingBatch3.ConsoleApp2
{
    public class ProductDapperService
    {
        SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder()
        {
            DataSource=".",
            InitialCatalog="MiniPOS",
            UserID="sa",
            Password = "sasa@123",
            TrustServerCertificate=true

        };


        public void Read() 
        {
            using(IDbConnection db = new SqlConnection(sqlConnectionStringBuilder.ConnectionString))
            {
                db.Open();

                db.Query
            }
        }
        public void Create() 
        {
        
        }
        public void Update() 
        {
        
        }
        public void Delete() 
        {
        
        }


    }

    

    }
