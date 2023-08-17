namespace TheEstate.Models.ResponseModels
{
    public class PaginatedResponseModel<T>
    {
        public int Total { get; set; }
        public T? Data { get; set; }
        
    }
}
