using AspNetScaffolding.Models.Request;
using System.Collections.Generic;

namespace AspNetScaffolding.Models.Response
{
    public class ListResponse<T>
    {
        public ListResponse()
        {
            this.Items = new List<T>();
            this.Paging = new PagingResponse();
        }

        public ListResponse(List<T> items, ListRequest request, int totalItems = 0)
        {
            this.Items = items;
            this.Paging = new PagingResponse(request, totalItems);
        }

        public List<T> Items { get; set; }

        public PagingResponse Paging { get; set; }
    }
}
