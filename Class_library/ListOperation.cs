using System;
using System.Collections;

namespace Class_library
{
    public class ListOperation: IEnumerable
    {
        public Operation FirstOperation { get; private set; }
        public int SumAmount { 
            get 
            {
                var sum = 0;
                foreach (Operation item in GetEnumeratorAmounts())
                {
                    sum += item.Amount;
                }
                return sum;
            } }
        /// <summary>
        /// Метод добавления операции
        /// </summary>
        /// <param name="amount">Сумма</param>
        /// <returns></returns>
        public bool AddOperation(int amount)
        {
            try
            {
                if (FirstOperation == null)
                {
                    FirstOperation = new Operation(amount);
                    FirstOperation.Next = FirstOperation;
                    FirstOperation.Previous = FirstOperation;
                    return true;
                }
                var newOperation = new Operation(amount);
                newOperation.Next = FirstOperation;
                newOperation.Previous = FirstOperation.Previous;
                FirstOperation.Previous.Next = newOperation;
                FirstOperation.Previous = newOperation;
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

            if(current != null)
            {
                newOperation.Next = current;
                newOperation.Previous = current;
                current.Previous.Next = newOperation;
                current.Previous = newOperation;
                return true;
            }
            return false;
        }


        public bool ChangeOperation(int operationId, int amount)
        {
            var currentOperation = FirstOperation;
            if (currentOperation == null)
                return false;
            do
            {
                if (operationId == currentOperation.OperationId)
                {
                    currentOperation.Amount = amount;
                    return true;
                }
            } while (currentOperation != FirstOperation);
            return false;
        }

        private Operation GetOperation(int operationId)
        {
            var currentOperation = FirstOperation;
            if (currentOperation == null)
                return null;
            do
            {
                if (operationId == currentOperation.OperationId)
                {
                    return currentOperation;
                }
            } while (currentOperation != FirstOperation);
            return null;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)this).GetEnumerator();
        }
        public IEnumerable GetEnumerator()
        {
            var currentOperation = FirstOperation;
            do
            {
                if (currentOperation != null)
                {
                    yield return currentOperation;
                    currentOperation = currentOperation.Next;
                }
            }
            while (currentOperation != FirstOperation);
        }

        public IEnumerable GetEnumeratorAmounts()
        {
            var currentOperation = FirstOperation;
            do
            {
                if (currentOperation != null)
                {
                    yield return currentOperation.Amount;
                    currentOperation = currentOperation.Next;
                }
            }
            while (currentOperation != FirstOperation);
        }
    }
}
