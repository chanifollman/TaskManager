using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager_data
{
    public class UserTaskRepository
    {
        private string _connectionString;
        public UserTaskRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Register(User user, string password)
        {
            string passwordSalt = PasswordHelper.GenerateSalt();
            string passwordHash = PasswordHelper.HashPassword(password, passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            using (var context = new userandtaskDataContext(_connectionString))
            {
                context.Users.InsertOnSubmit(user);
                context.SubmitChanges();
            }
        }
        public User Login(string email, string password)
        {
            using (var context = new userandtaskDataContext(_connectionString))
            {
                var user = GetByEmail(email);
                if (user == null)
                {
                    return null;
                }
                bool isCorrectPassword = PasswordHelper.PasswordMatch(password, user.PasswordSalt, user.PasswordHash);
                if (isCorrectPassword)
                {
                    return user;
                }

                return null;
            }
        }
        public void SelectTask(int userId, int taskId)
        {
            using (var context = new userandtaskDataContext(_connectionString))
            {
                var task = context.Tasks.FirstOrDefault(t => t.Id == taskId);
                task.UserId = userId;
                task.Status = Status.Taken;
                context.SubmitChanges();
            }
        }
        public void CompleteTask(int id)
        {
            using (var context = new userandtaskDataContext(_connectionString))
            {
                var task = context.Tasks.FirstOrDefault(t => t.Id == id);
                task.Status = Status.Completed;
                context.SubmitChanges();
            }
        }
        public User GetByEmail(string email)
        {
            using (var context = new userandtaskDataContext(_connectionString))
            {
                return context.Users.FirstOrDefault(u => u.Email == email);
            }
        }
        public User GetUserById(int id)
        {
            using (var context = new userandtaskDataContext(_connectionString))
            {
                return context.Users.FirstOrDefault(u => u.Id == id);
            }
        }
        public IEnumerable<Task> GetTasks()
        {
            using (var context = new userandtaskDataContext(_connectionString))
            {
                var loadOptions = new DataLoadOptions();
                loadOptions.LoadWith<Task>(t => t.User);
                context.LoadOptions = loadOptions;
                return context.Tasks.ToList();
            }
        }
        public void AddTask(Task task)
        {
            using (var context = new userandtaskDataContext(_connectionString))
            {
                context.Tasks.InsertOnSubmit(task);
                context.SubmitChanges();
            }
        }
        public static class PasswordHelper
        {
            public static string GenerateSalt()
            {
                RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
                byte[] bytes = new byte[10];
                provider.GetBytes(bytes);
                return Convert.ToBase64String(bytes);
            }

            public static string HashPassword(string password, string salt)
            {
                SHA256Managed crypt = new SHA256Managed();
                string combinedString = password + salt;
                byte[] combined = Encoding.Unicode.GetBytes(combinedString);

                byte[] hash = crypt.ComputeHash(combined);
                return Convert.ToBase64String(hash);
            }

            public static bool PasswordMatch(string userInput, string salt, string passwordHash)
            {
                string userInputHash = HashPassword(userInput, salt);
                return passwordHash == userInputHash;
            }

        }


    }
}
