﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;


namespace BlogBuster.Models
{
    public class User
    {
        [Required]
        public int Id { get; private set; }

        [Display(Name = "Full name")]
        [Required]
        [StringLength(100)]
        public string Name { get; set; }


        [Display(Name = "Email address")]
        [Required(ErrorMessage = "The email address is required")]
        [StringLength(254)]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "The password is required")]
        [StringLength(254)]
        public string Password { get; set; }

        [Required]
        [StringLength(10)]
        public Gender Gender { get; set; }

        [Required]
        public Type Type { get; set; }

        public List<MicroPost> MicroPosts { get; }
        public List<Movie> Movies { get; set; }
        public List<Book> Books { get; set; }
        public User(string Name = null, string Email = null, string Password = null, string Gender = null, string Type = null)
        {
            this.Name = Name;
            this.Email = Email;
            this.Password = Password;
            this.MicroPosts = new List<MicroPost>();
            this.Movies = new List<Movie>();
            this.Books = new List<Book>();

            this.GetMicroPosts();
            this.GetMovies();
            this.GetBooks();

            switch (Gender)
            {
                case "Male":
                    this.Gender = Models.Gender.Male;
                    break;
                case "Female":
                    this.Gender = Models.Gender.Female;
                    break;
                default:
                    this.Gender = Models.Gender.Male;
                    break;
            }

            switch (Type)
            {
                case "Administrator":
                    this.Type = Models.Type.Administrator;
                    break;
                case "Normal":
                    this.Type = Models.Type.Normal;
                    break;
                default:
                    this.Type = Models.Type.Normal;
                    break;
            }
        }

        public User(int Id, string Name = null, string Email = null, string Password = null, string Gender = null, string Type = null)
        {
            this.Id = Id;
            this.Name = Name;
            this.Email = Email;
            this.Password = Password;
            this.MicroPosts = new List<MicroPost>();
            this.Movies = new List<Movie>();
            this.Books = new List<Book>();

            this.GetMicroPosts();
            this.GetMovies();
            this.GetBooks();

            switch (Gender)
            {
                case "Male":
                    this.Gender = Models.Gender.Male;
                    break;
                case "Female":
                    this.Gender = Models.Gender.Female;
                    break;
                default:
                    this.Gender = Models.Gender.Male;
                    break;
            }

            switch (Type)
            {
                case "Administrator":
                    this.Type = Models.Type.Administrator;
                    break;
                case "Normal":
                    this.Type = Models.Type.Normal;
                    break;
                default:
                    this.Type = Models.Type.Normal;
                    break;
            }
        }

        public static List<User> All()
        {
            List<User> users = new List<User>();

            DataTable dataTable = SqlConnectionGenerator.ExecuteStoreProcedure("sp_show_all_users");

            for (int row = 0; row < dataTable.Rows.Count; row++)
            {
                users.Add(new User
                (
                    int.Parse(dataTable.Rows[row][0].ToString()),
                    dataTable.Rows[row][1].ToString(),
                    dataTable.Rows[row][2].ToString(),
                    dataTable.Rows[row][3].ToString(),
                    dataTable.Rows[row][4].ToString(),
                    dataTable.Rows[row][5].ToString()
                ));
            }

            return users;
        }

        public static User Find(int Id)
        {
            DataTable dataTable = SqlConnectionGenerator.ExecuteStoreProcedure("sp_show_user",
                new string[] { "@User_Id" },
                new object[] { Id }
                );

            return new User(
                int.Parse(dataTable.Rows[0][0].ToString()),
                dataTable.Rows[0][1].ToString(),
                dataTable.Rows[0][2].ToString(),
                dataTable.Rows[0][3].ToString(),
                dataTable.Rows[0][4].ToString(),
                dataTable.Rows[0][5].ToString()
                );
        }



        public static User FindBy(string Email, string Password)
        {
            DataTable dataTable = SqlConnectionGenerator.ExecuteStoreProcedure("sp_show_user_by_credentials",
                new string[] { "@Email", "@Password" },
                new object[] { Email, Password }
                );

            return new User(
                int.Parse(dataTable.Rows[0][0].ToString()),
                dataTable.Rows[0][1].ToString(),
                dataTable.Rows[0][2].ToString(),
                dataTable.Rows[0][3].ToString(),
                dataTable.Rows[0][4].ToString(),
                dataTable.Rows[0][5].ToString()
                );
        }

        private void GetBooks()
        {
            if (this.Id != 0)
            {
                DataTable dataTable = SqlConnectionGenerator.ExecuteStoreProcedure("sp_show_user_books",
                    new string[] { "@User_Id" },
                    new object[] { Id }
                );

                for (int row = 0; row < dataTable.Rows.Count; row++)
                {
                    Books.Add(new Book
                    (
                        int.Parse(dataTable.Rows[row][2].ToString()),
                        dataTable.Rows[row][3].ToString(),
                        dataTable.Rows[row][4].ToString(),
                        int.Parse(dataTable.Rows[row][0].ToString())
                    ));
                }
            }
        }

        private void GetMicroPosts()
        {
            if (this.Id != 0)
            {
                DataTable dataTable = SqlConnectionGenerator.ExecuteStoreProcedure("sp_show_user_microposts",
                    new string[] { "@User_Id" },
                    new object[] { Id }
                );

                for (int row = 0; row < dataTable.Rows.Count; row++)
                {
                    MicroPosts.Add(new MicroPost
                    (
                        int.Parse(dataTable.Rows[row][2].ToString()),
                        dataTable.Rows[row][3].ToString(),
                        int.Parse(dataTable.Rows[row][0].ToString())
                    ));
                }
            }
        }

        private void GetMovies()
        {
            if (this.Id != 0)
            {
                DataTable dataTable = SqlConnectionGenerator.ExecuteStoreProcedure("sp_show_user_movies",
                    new string[] { "@User_Id" },
                    new object[] { Id }
                );

                for (int row = 0; row < dataTable.Rows.Count; row++)
                {
                    Movies.Add(new Movie
                    (
                        int.Parse(dataTable.Rows[row][2].ToString()),
                        dataTable.Rows[row][3].ToString(),
                        dataTable.Rows[row][4].ToString(),
                        int.Parse(dataTable.Rows[row][0].ToString())
                    ));
                }
            }
        }

        public bool Save()
        {
            try
            {
                if (ItExists())
                    SqlConnectionGenerator.ExecuteStoreProcedure("sp_update_user",
                        new string[] { "@User_Id", "@Name", "@Email", "@Password", "@Gender" },
                        new object[] { this.Id, this.Name, this.Email, this.Password, this.Gender.ToString() }
                        );
                else
                    SqlConnectionGenerator.ExecuteStoreProcedure("sp_create_user",
                    new string[] { "@Name", "@Email", "@Password", @"Gender", "@Type" },
                    new object[] { this.Name, this.Email, this.Password, this.Gender.ToString(), this.Type.ToString() }
                    );

                return true;
            }
            catch (System.Exception)
            {
                return false;
            }

        }

        public bool Delete()
        {
            try
            {
                SqlConnectionGenerator.ExecuteStoreProcedure("sp_delete_user",
                    new string[] { "@User_Id" },
                    new object[] { this.Id }
                    );
                return true;
            }
            catch (System.Exception)
            {
                return false;
            }
        }

        private bool ItExists()
        {
            try
            {
                return (User.Find(this.Id).Id > 0);
            }
            catch (System.Exception)
            {
                return false;
            }
        }
    }

    public enum Gender
    {
        Male,
        Female
    }

    public enum Type
    {
        Administrator,
        Normal
    }
}