using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;

namespace urTribeWebAPI.DAL.Factory
{
    public sealed class RepositoryFactory
    {
        #region Member Variables
        private static readonly RepositoryFactory _instance = new RepositoryFactory ();
        private readonly Dictionary<Type, Type> _repositoryTypeMap;
        #endregion

        #region Properties
        public static RepositoryFactory Instance
        {
            get
            {
                return _instance;
            }
        }
        #endregion

        #region Constructors
        private RepositoryFactory ()
        {
            _repositoryTypeMap = LoadRepositoryTypes();
        }
        #endregion

        #region Private Methods
        private Dictionary<Type, Type> LoadRepositoryTypes()
        {
            using (StreamReader r = new StreamReader("RepositoryMap.json"))
            {
                string json = r.ReadToEnd();
                Dictionary<Type, Type> repositoryTypeMap = JsonConvert.DeserializeObject<Dictionary<Type, Type>>(json);
                return repositoryTypeMap;
            }
        }
        #endregion

        #region Public Methods
        public repositorytype Create<repositorytype>() where repositorytype : class
        {
            Type t = typeof(repositorytype);
            var repository = Activator.CreateInstance(_repositoryTypeMap[t]);
            repositorytype concrete = repository as repositorytype;
            return concrete;
        }
        #endregion
    }
}
