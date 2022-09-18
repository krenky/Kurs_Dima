using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json.Serialization;

namespace Class_library
{
    public class ListOperation : IEnumerable<Operation>
    {
        private Operation firstOperation;
        private ObservableCollection<Operation> operations;

        public ListOperation()
        {
            this.firstOperation = null;
            operations = new ObservableCollection<Operation>();
        }

        private Operation FirstOperation { get => firstOperation; set => firstOperation = value; }
        public ObservableCollection<Operation> Operations { get => operations; 
            set 
            { 
                var list = new ObservableCollection<Operation>(value.Where(x => x != null));
                //operations = list; 
                firstOperation = null;
                foreach(var operation in list)
                {
                    AddOperation(operation);
                }
            } 
        }
        [JsonIgnore]
        public int SumAmount
        {
            get
            {
                var sum = 0;
                if (firstOperation == null)
                    return sum;
                foreach (Operation item in this.ToList())
                {
                    sum += item.Amount;
                }
                return sum;
            }
        }
        /// <summary>
        /// Метод добавления операции
        /// </summary>
        /// <param name="amount">Сумма</param>
        /// <returns></returns>
        public bool AddOperation(int amount)
        {
            try
            {
                if (firstOperation == null)
                {
                    firstOperation = new Operation(amount);
                    firstOperation.Next = firstOperation;
                    firstOperation.Previous = firstOperation;
                    operations.Add(firstOperation);
                    return true;
                }
                var newOperation = new Operation(amount);
                newOperation.Next = firstOperation;
                newOperation.Previous = firstOperation.Previous;
                firstOperation.Previous.Next = newOperation;
                firstOperation.Previous = newOperation;
                if (operations == null)
                    operations = new ObservableCollection<Operation>();
                operations.Add(newOperation);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Метод добавления операции
        /// Для сериализации
        /// </summary>
        /// <param name="amount">Сумма</param>
        /// <returns></returns>
        public bool AddOperation(Operation operation)
        {
            try
            {
                if (firstOperation == null)
                {
                    firstOperation = new Operation(operation.OperationId, operation.DateOperation, operation.Amount);
                    firstOperation.Next = firstOperation;
                    firstOperation.Previous = firstOperation;
                    operations.Add(firstOperation);
                    return true;
                }
                var newOperation = new Operation(operation.OperationId, operation.DateOperation, operation.Amount);
                newOperation.Next = firstOperation;
                newOperation.Previous = firstOperation.Previous;
                firstOperation.Previous.Next = newOperation;
                firstOperation.Previous = newOperation;
                if (operations == null)
                    operations = new ObservableCollection<Operation>();
                operations.Add(newOperation);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool AddBeforeOperation(int amount, int operatiomId)
        {
            var newOperation = new Operation(amount);
            var current = GetOperation(operatiomId);

            if (firstOperation == null)
            {
                firstOperation = newOperation;
                firstOperation.Next = firstOperation;
                firstOperation.Previous = firstOperation;
                operations = GetEnumerator() as ObservableCollection<Operation>;
                return true;
            }

            if (current != null)
            {
                newOperation.Next = current;
                newOperation.Previous = current.Previous;
                current.Previous.Next = newOperation;
                current.Previous = newOperation;
                operations = new ObservableCollection<Operation>(this);
                return true;
            }
            return false;
        }

        public bool AddAfterOperation(int amount, int operatiomId)
        {
            var newOperation = new Operation(amount);
            var current = GetOperation(operatiomId);

            if (firstOperation == null)
            {
                firstOperation = newOperation;
                firstOperation.Next = firstOperation;
                firstOperation.Previous = firstOperation;
                operations = new ObservableCollection<Operation>(this);
                return true;
            }

            if (current != null)
            {
                newOperation.Next = current.Next;
                newOperation.Previous = current;
                current.Next.Previous = newOperation;
                current.Next = newOperation;
                operations = new ObservableCollection<Operation>(this);
                return true;
            }
            return false;
        }

        public bool ChangeOperation(int operationId, int amount)
        {
            var currentOperation = firstOperation;
            if (currentOperation == null)
                return false;
            do
            {
                if (operationId == currentOperation.OperationId)
                {
                    currentOperation.Amount = amount;
                    return true;
                }
            } while (currentOperation != firstOperation);
            return false;
        }

        private Operation GetOperation(int operationId)
        {
            var currentOperation = firstOperation;
            if (currentOperation == null)
                return null;
            do
            {
                if (operationId == currentOperation.OperationId)
                {
                    return currentOperation;
                }
                currentOperation = currentOperation.Next;
            } while (currentOperation != firstOperation);
            return null;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            var currentOperation = firstOperation;
            do
            {
                if (currentOperation != null)
                {
                    yield return currentOperation;
                    currentOperation = currentOperation.Next;
                }
            }
            while (currentOperation != firstOperation);
        }
        public IEnumerator<Operation> GetEnumerator()
        {
            var currentOperation = firstOperation;
            do
            {
                if (currentOperation != null)
                {
                    yield return currentOperation;
                    currentOperation = currentOperation.Next;
                }
            }
            while (currentOperation != firstOperation);
        }
    }
}
