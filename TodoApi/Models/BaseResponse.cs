namespace TodoApiDTO.Models
{
    public class BaseResponse<T> where T : class
    {
        /// <summary>
        /// Код ответа
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// Сообщение
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public T Data { get; set; }
    }
}
