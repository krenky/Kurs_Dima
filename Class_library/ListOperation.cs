using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json.Serialization;

namespace Class_library
{
    /// <summary>
    /// Класс списка операций
    /// </summary>
    public class ListOperation : IEnumerable<Operation>
    {
        private Operation firstOperation;
        private ObservableCollection<Operation> operations;
        /// <summary>
        /// контсруктор
        /// </summary>
        public ListOperation()
        {
            this.firstOperation = null;
            operations = new ObservableCollection<Operation>();
        }

        private Operation FirstOperation { get => firstOperation; set => firstOperation = value; }
        /// <summary>
        /// Список операций
        /// !!!Для привязки к гриду!!!
        /// </summary>
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
        /// <summary>
        /// Сумма операция клиента
        /// </summary>
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
        /// <summary>
        /// Изменить сумму операции
        /// </summary>
        /// <param name="operationId">Id операции которую надо изменить</param>
        /// <param name="amount">Сумма</param>
        /// <returns>True - Успешное изменение, False - операция не найдена</returns>
        public bool ChangeOperation(int operationId, int amount)
        {
            var currentOperation = firstOperation;
            if (currentOperation == null)
                return false;
            do
            {
                currentOperation = currentOperation.Next;
                if (operationId == currentOperation.OperationId)
                {
                    currentOperation.Amount = amount;
                    currentOperation.DateOperation = DateTime.Now;
                    currentOperation.Previous.Next = currentOperation.Next;
                    currentOperation.Next.Previous = currentOperation.Previous;
                    operations.Remove(currentOperation);
                    AddOperation(currentOperation);
                    return true;
                }
            } while (currentOperation != firstOperation);
            return false;
        }
        /// <summary>
        /// реализация интерфейса ienumerator
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// реализация интерфейса ienumerator
        /// </summary>
        /// <returns></returns>
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
