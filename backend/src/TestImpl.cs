using System.Threading.Tasks;

namespace backend
{
    public class TestImpl : Test.TestBase
    {
        public TestImpl(){}

        public override Task<Response> GetValue(Request request, Grpc.Core.ServerCallContext context) 
        {
            return Task.FromResult(new Response { Result = "OK"});
        }

    }
    
}