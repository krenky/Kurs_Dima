using System;
using System.Text.Json.Serialization;

namespace Class_library
{
    /// <summary>
    /// Класс операции
    /// </summary>
    public class Operation
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public Operation()
        {
            OperationId = lastId;
            lastId++;
        }
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="amount">Сумма</param>
        public Operation(int amount)
        {
            DateOperation = DateTime.Now;
            Amount = amount;
            OperationId = lastId;
            lastId++;
        }
        /// <summary>
        /// Коструктор
        /// </summary>
        /// <param name="dateOperation">Дата операции</param>
        /// <param name="amount">Сумма</param>
        public Operation(DateTime dateOperation, int amount)
        {
            DateOperation = dateOperation;
            Amount = amount;
            OperationId = lastId;
            lastId++;
        }
        /// <summary>
        /// Для сериализации
        /// </summary>
        /// <param name="operationId">Id операции</param>
        /// <param name="dateOperation">Дата операции</param>
        /// <param name="amount">Сумма</param>
        public Operation(int operationId, DateTime dateOperation, int amount)
        {
            OperationId = operationId;
            DateOperation = dateOperation;
            Amount = amount;
        }

        private static int lastId = 0;
        /// <summary>
        /// Id операции
        /// </summary>
        public int OperationId { get; set; }
        /// <summary>
        /// Ссылка на следующую операцию
        /// </summary>
        [JsonIgnore]
        public Operation Next { get; set; }
        /// <summary>
        /// Ссылка на предыдущую операци
        /// </summary>
        [JsonIgnore]
        public Operation Previous { get; set; }
        /// <summary>
        /// Даты операции
        /// </summary>
        public DateTime DateOperation { get; set; } = DateTime.MinValue;
        /// <summary>
        /// Сумма операции
        /// </summary>
        public int Amount { get; set; }
    }
}
