using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using urTribeWebAPI.Common.Logging;
using System.Configuration;

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
            try
            {
                string MapLocation = ConfigurationManager.AppSettings["RepositoryMapFileLocation"];
                using (StreamReader r = new StreamReader(MapLocation))
                {
                    string json = r.ReadToEnd();
                    Dictionary<Type, Type> repositoryTypeMap = JsonConvert.DeserializeObject<Dictionary<Type, Type>>(json);
                    return repositoryTypeMap;
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Log = new ExceptionDTO() { FaultClass = "RepositoryFactory", FaultMethod = "LoadRepositoryTypes", Exception = ex };
                return new Dictionary<Type, Type> ();
            }
        }
        #endregion

        #region Public Methods
        public repositorytype Create<repositorytype>() where repositorytype : class
        {
            try
            {
                Type t = typeof(repositorytype);
                var repository = Activator.CreateInstance(_repositoryTypeMap[t]);
                repositorytype concrete = repository as repositorytype;
                return concrete;
            }
            catch (Exception ex)
            {
                throw new RepositoryNotExistException(ex);
            }
        }
        #endregion
    }
}
