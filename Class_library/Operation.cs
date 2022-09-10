using System;

namespace Class_library
{
    public class Operation
    {
        public Operation()
        {
            OperationId = lastId;
            lastId++;
        }

        public Operation(int amount)
        {
            DateOperation = DateTime.Now;
            Amount = amount;
            OperationId = lastId;
            lastId++;
        }

        public Operation(DateTime dateOperation, int amount)
        {
            DateOperation = dateOperation;
            Amount = amount;
            OperationId = lastId;
            lastId++;
        }
        
        private static int lastId = 0;
        public int OperationId { get; set; }
        public Operation Next { get; set; }
        public Operation Previous { get; set; }
        public DateTime DateOperation { get; set; } = DateTime.MinValue;
        public int Amount { get; set; }
    }
}
