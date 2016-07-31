using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Raven.Client.Documents;
using Raven.Client.Documents.Commands;
using Sparrow.Json;

namespace Tryouts
{
    public class TestClient
    {
        public async Task Test()
        {
            using (var store = new DocumentStore
            {
                Url = "http://localhost.fiddler:8079",
                DefaultDatabase = "ClientNew"
            }.Initialize())
            {
                // Should be part of the store
                var requestExecuter = new RequestExecuter();
                var context = new JsonOperationContext(store.UnmanagedBuffersPool);
                // TODO: Dispose, only if it not cached. If cached, dispose when expriting the cache.

                var putCommand = new PutDocumentCommand
                {
                    Context = context,
                    Url = store.Url,
                    Database = store.DefaultDatabase,
                    Id = "users/1",
                    Document = ConvertObjectToBlittable(new { Name = "Fitzchak Yitzchaki" }, context)
                };
                await requestExecuter.ExecuteCommandAsync(putCommand, context);

                var getCommand = new GetDocumentCommand
                {
                    Context = context,
                    Url = store.Url,
                    Database = store.DefaultDatabase,
                    Id = "users/1",
                };
                await requestExecuter.ExecuteCommandAsync(getCommand, context);

                getCommand = new GetDocumentCommand
                {
                    Context = context,
                    Url = store.Url,
                    Database = store.DefaultDatabase,
                    Id = "users/1",
                };
                await requestExecuter.ExecuteCommandAsync(getCommand, context);
            }
        }

        public BlittableJsonReaderObject ConvertObjectToBlittable(object value, JsonOperationContext context)
        {
            var json = JsonConvert.SerializeObject(value);
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                var blittableJsonReaderObject = context.ReadForMemory(stream, "doc");
                return blittableJsonReaderObject;
            }
        }
    }
}