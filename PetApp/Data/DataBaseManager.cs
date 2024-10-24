using PetApp.DbConnection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetApp.Data
{
    internal static class DataBaseManager
    {
        private static PetAppEntities _dbConnection = new PetAppEntities();

        public static bool UpdateDatabase()
        {
            try
            {
                _dbConnection.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static List<Users> GetUsers()
        {
            return _dbConnection.Users.ToList();
        }

        public static bool AddUser(Users u)
        {
            try
            {
                _dbConnection.Users.Add(u);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static Users AuthenticateUser(string username, string password)
        {
            using (var db = new PetAppEntities())
            {
                var user = db.Users.FirstOrDefault(u => u.username == username && u.password == password);

                return user;
            }
        }

        public static List<Roles> GetRoles()
        {
            return _dbConnection.Roles.ToList();
        }

        public static bool UpdateUser(Users user)
        {
            try
            {
                var existingUser = _dbConnection.Users.FirstOrDefault(u => u.id_user == user.id_user);
                if (existingUser != null)
                {
                    existingUser.username = user.username;
                    existingUser.password = user.password;

                    _dbConnection.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public static bool UserExists(string username)
        {
            return _dbConnection.Users.Any(u => u.username == username);
        }

        public static Pet GetPetByUserId(int userId)
        {
            using (var db = new PetAppEntities())
            {
                return db.Pet.FirstOrDefault(p => p.id_user == userId);
            }
        }

        public static List<Pet> GetAllPets()
        {
            using (var db = new PetAppEntities())
            {
                return db.Pet.ToList();
            }
        }
    }
}