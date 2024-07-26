namespace Talabat.APIs.Helpers
{
	public class Pagination<T>
	{

		public Pagination(int pageSize, int pageIndex, int pageCount, IReadOnlyList<T> data)
		{
			PageSize = pageSize;
			PageIndex = pageIndex;
			Count = pageCount;
			Data = data;
		}


		public int PageSize { get; set; }
		
		public int PageIndex { get; set; }

		public int Count { get; set; }

		public IReadOnlyList<T> Data { get; set; }


	}
}
