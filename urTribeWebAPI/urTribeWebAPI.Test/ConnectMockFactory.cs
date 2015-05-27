using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Newtonsoft.Json;
using urTribeWebAPI.Messaging;
using urTribeWebAPI.Messaging.RTFHelperClasses;

namespace urTribeWebAPI.Test
{
    class ConnectMockFactory 
    {

        private static Mock<IMessageConnect> mockConnect;
        private static string creationURL = urTribeWebAPI.Messaging.Properties.Settings.Default.RTFCreateURL;
        private static Expression<Func<string, bool>> isCreation = url => url.Equals(creationURL);
        private static string authURL = urTribeWebAPI.Messaging.Properties.Settings.Default.RTFAuthURL;
        private static Expression<Func<string, bool>> isAuth = (url => url.Equals(authURL));
        private static Func<string, string> creationQueryToResponse =
                q => JsonConvert.SerializeObject(new CreationResponse()
                {
                    data = new CreationDataResponse()
                    {
                        table = JsonConvert.DeserializeObject<CreateTableQuery>(q).table,
                        creationDate = "February 99th, 9999",
                        status = "Creating"
                    }
                });

        public static Mock<IMessageConnect> newMock()
        {
            mockConnect = new Mock<IMessageConnect>();
            ErrorResponse error = new ErrorResponse() { code = "-1", message = "unhelpful error message" };
            string serializedError = JsonConvert.SerializeObject(error);
            mockConnect.Setup(foo => foo.SendRequest(It.IsAny<string>(), null)).Returns(serializedError);
            mockConnect.Setup(foo => foo.SendRequest(null, It.IsAny<string>())).Returns(serializedError);
            mockConnect.Setup(foo => foo.CreationSleepTime()).Returns(0);

            //Table creation queries will return a well formatted response with their table name
            //This means malformed queries will throw an exception.
            mockConnect.Setup(foo => foo.SendRequest(It.Is(isCreation), It.IsAny<string>()))
                .Returns((string u, string d) => creationQueryToResponse(d));

            //Non null Auth queries always return true. Malformed queries will not throw exceptions
            mockConnect.Setup(foo => foo.SendRequest(It.Is(isAuth), It.IsAny<string>()))
                .Returns(JsonConvert.SerializeObject(new BooleanResponse()
                {
                    data = true
                }));

            return mockConnect;
        }
    }
}
