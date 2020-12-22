

using System.Threading.Tasks;

namespace JoKenPo.Domain.Interfaces
{
    public interface IHttpService
    {
        public Task<T> ExecuteAsync<T>();
        public IHttpService AddHeader(string name, object value);
        public IHttpService AddBody(string name, object value);
        public IHttpService AddQuery(string name, object value);
        public IHttpService AddPath(string name, object value);
        public IHttpService Get(string url);
        public IHttpService Post(string url);
        public IHttpService Delete(string url);
        public IHttpService AddParameter(string Type, string name, object value);
    }
}