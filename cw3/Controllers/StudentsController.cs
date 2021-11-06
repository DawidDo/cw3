using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.RegularExpressions;
using cw3.Models;
using Microsoft.AspNetCore.Mvc;

namespace cw3.Controllers
{
    [Route("api/students")]
    [ApiController]
    public class StudentsController : Controller
    {
        
        
        [HttpGet("{indexNumber}")]
        public IActionResult GetStudent(string indexNumber)
        {
            string csvPath = "/Users/dawiddolowy/Desktop/Studia/s7/APBDx2/workplaceCW1/cw3/cw3/Data/studenci.csv";
            FileInfo fi = new FileInfo(csvPath);
            StreamReader streamReader = new StreamReader(fi.OpenRead());
            string line = null;
            List<Student> studenci = new List<Student>();
           
                while ((line = streamReader.ReadLine()) != null)
                {
                    string[] atr = line.Split(",");

                    if (atr[2] == indexNumber)
                    {
                        studenci.Add(new Student
                        {
                            FirstName = atr[0],
                            LastName = atr[1],
                            Number = atr[2],
                            Data = atr[3],
                            Kierunek = atr[4],
                            Tryb = atr[5],
                            Mail = atr[6],
                            Ojciec = atr[7],
                            Matka = atr[8]
                        });
                    }
                }
                

            var json = JsonSerializer.Serialize(studenci);

            return Ok(json);
        }
        
        
        [HttpGet()]
        public IActionResult GetStudents()
        {
            string csvPath = "/Users/dawiddolowy/Desktop/Studia/s7/APBDx2/workplaceCW1/cw3/cw3/Data/studenci.csv";
            FileInfo fi = new FileInfo(csvPath);
            StreamReader streamReader = new StreamReader(fi.OpenRead());
            string line = null;
            List<Student> studenci = new List<Student>();
            
                while ((line = streamReader.ReadLine()) != null)
                {
                    string[] atr = line.Split(",");
                    
                    studenci.Add(new Student
                    {
                        FirstName = atr[0],
                        LastName = atr[1],
                        Number = atr[2],
                        Data = atr[3],
                        Kierunek = atr[4],
                        Tryb = atr[5],
                        Mail = atr[6],
                        Ojciec = atr[7],
                        Matka = atr[8]
                    });
                    
                    
                }

                var json = JsonSerializer.Serialize(studenci);

            return Ok(json);
        }

        [HttpPut("{indexNumber}")]
        public IActionResult AddStudent([FromBody]Student student, [FromRoute]string indexNumber)
        {
            string csvPath = "/Users/dawiddolowy/Desktop/Studia/s7/APBDx2/workplaceCW1/cw3/cw3/Data/studenci.csv";
            FileInfo fi = new FileInfo(csvPath);
            StreamReader streamReader = new StreamReader(fi.OpenRead());
            string line = null;
            List<Student> studenci = new List<Student>();
            Student s = null;
            while ((line = streamReader.ReadLine()) != null)
            {
                string[] atr = line.Split(",");
                
                if (atr[2] == indexNumber)
                {
                    s  = student;
                    studenci.Add(s);
                }
                else
                {
                    studenci.Add(new Student
                    {
                        FirstName = atr[0],
                        LastName = atr[1],
                        Number = atr[2],
                        Data = atr[3],
                        Kierunek = atr[4],
                        Tryb = atr[5],
                        Mail = atr[6],
                        Ojciec = atr[7],
                        Matka = atr[8]
                    });
                }
            }
            streamReader.Close();
            fi.Delete();
            
            StreamWriter stream = new StreamWriter(fi.OpenWrite());
            foreach (Student stud in studenci)
            {
                stream.Write(JsonSerializer.Serialize(stud));
                stream.WriteLine(" ");
            }
            stream.Close();
            var json = JsonSerializer.Serialize(s);
            
            return Ok(json);
        }

        [HttpPost]
        public IActionResult PostStudent(Student s)
        {
            if (s.Data.Length > 1 && s.Kierunek.Length > 1 && s.Mail.Length > 1 && s.Matka.Length > 1 &&
                s.Number.Length > 1
                && s.Ojciec.Length > 1 && s.Tryb.Length > 1 && s.FirstName.Length > 1 && s.LastName.Length > 1)
            {
                string csvPath = "/Users/dawiddolowy/Desktop/Studia/s7/APBDx2/workplaceCW1/cw3/cw3/Data/studenci.csv";
                FileInfo fi = new FileInfo(csvPath);
                StreamReader streamReader = new StreamReader(fi.OpenRead());
                string line = null;
                List<Student> studenci = new List<Student>();

                while ((line = streamReader.ReadLine()) != null)
                {

                    string pattern = @"s[0-9]+";
                    Regex rg = new Regex(pattern);
                    bool correctNumber = rg.IsMatch(s.Number);

                    if (correctNumber == false)
                        return StatusCode(401, "Błędny numer studenta");

                    string[] atr = line.Split(",");
                    
                    if (atr[2] == s.Number)
                    {
                        return StatusCode(402, "Student o tym numerze indeksu już istnieje");
                    }
                    
                    studenci.Add(new Student
                    {
                        FirstName = atr[0],
                        LastName = atr[1],
                        Number = atr[2],
                        Data = atr[3],
                        Kierunek = atr[4],
                        Tryb = atr[5],
                        Mail = atr[6],
                        Ojciec = atr[7],
                        Matka = atr[8]
                        
                    });

                }
                
                studenci.Add(s);
                
                streamReader.Close();
                fi.Delete();
                
                StreamWriter stream = new StreamWriter(fi.OpenWrite());
                foreach (Student stud in studenci)
                {
                    stream.Write(JsonSerializer.Serialize(stud));
                    stream.WriteLine(" ");
                }
                stream.Close();

            }
            else
                return StatusCode(401, "Podane dane są niekompletne");

            return Ok();
        }


        [HttpDelete("{number}")]
        public IActionResult DeleteStudent([FromRoute] string number)
        {
            string csvPath = "/Users/dawiddolowy/Desktop/Studia/s7/APBDx2/workplaceCW1/cw3/cw3/Data/studenci.csv";
            FileInfo fi = new FileInfo(csvPath);
            StreamReader streamReader = new StreamReader(fi.OpenRead());
            string line = null;
            List<Student> studenci = new List<Student>();
            bool deleted = false;
            while ((line = streamReader.ReadLine()) != null)
            {
                string[] atr = line.Split(",");

                if (atr[2] != number)
                {
                    studenci.Add(new Student
                    {
                        FirstName = atr[0],
                        LastName = atr[1],
                        Number = atr[2],
                        Data = atr[3],
                        Kierunek = atr[4],
                        Tryb = atr[5],
                        Mail = atr[6],
                        Ojciec = atr[7],
                        Matka = atr[8]

                    });
                }
                else
                    deleted = true;
            }
            
            streamReader.Close();
            fi.Delete();

            if (deleted == false)
            {
                return StatusCode(403, "Nie ma studenta o tym numerze indexu");
            }

            StreamWriter stream = new StreamWriter(fi.OpenWrite());
            foreach (Student stud in studenci)
            {
                stream.WriteLine(JsonSerializer.Serialize(stud));
              
            }
            stream.Close();

            return Ok();
        }


    }
}