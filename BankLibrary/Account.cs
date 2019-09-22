namespace BankLibrary
{
    public abstract class Account : IAccount
    {
        //Событие, возникающее при выводе денег
        protected internal AccountStateHandler Withdrawed;
        // Событие возникающее при добавление на счет
        protected internal AccountStateHandler Added;
        // Событие возникающее при открытии счета
        protected internal AccountStateHandler Opened;
        // Событие возникающее при закрытии счета
        protected internal AccountStateHandler Closed;
        // Событие возникающее при начислении процентов
        protected internal AccountStateHandler Calculated;

        protected int _id;
        static int counter = 0;

        protected decimal _sum; // Переменная для хранения суммы
        protected int _percentage; // Переменная для хранения процента

        protected int _days = 0; // время с момента открытия счета

        public Account(decimal sum, int percentage)
        {
            _sum = sum;
            _percentage = percentage;
            _id = ++counter;            
        }

        // Текущая сумма на счету
        public decimal CurrentSum
        {
            get { return _sum; }
        }
        public int Persentage
        {
            get { return _percentage; }
        }
        public int Id
        {
            get { return _id; }
        }

        // вызов событий
        private void CallEvent(AccountEventArgs e, AccountStateHandler handler)
        {
            if (handler != null && e != null)
                handler(this, e);
        }
        // вызов отдельных событий. Для каждого события определяется свой витуальный метод
        protected virtual void OnOpened(AccountEventArgs e)
        {
            CallEvent(e, Opened);
        }
        protected virtual void OnWithdrawed(AccountEventArgs e)
        {
            CallEvent(e, Withdrawed);
        }
        protected virtual void OnAdded(AccountEventArgs e)
        {
            CallEvent(e, Added);
        }
        protected virtual void OnClosed(AccountEventArgs e)
        {
            CallEvent(e, Closed);
        }
        protected virtual void OnCalculated(AccountEventArgs e)
        {
            CallEvent(e, Calculated);
        }

        public virtual void Put(decimal sum)
        {
            _sum += sum;
            OnAdded(new AccountEventArgs($"На счет поступило {sum}", sum));
        }
        public virtual decimal Withdraw(decimal sum)
        {
            decimal result = 0;
            if (sum <= _sum)
            {
                _sum -= sum;
                result = sum;
                OnWithdrawed(new AccountEventArgs($"Сумма {sum} снята со счета {_id}", sum));
            }
            else
            {
                OnWithdrawed(new AccountEventArgs($"Недостаточно денег на счете {_id}",0));
            }
            return result;
        }
        // открытие счета
        protected internal virtual void Open()
        {
            OnOpened(new AccountEventArgs($"Открыт новый счет! Id счета: {this._id}", this._id));
        }
        // закрытие счета
        protected internal virtual void Close()
        {
            OnClosed(new AccountEventArgs($"Счет { _id} закрыт.  Итоговая сумма: {CurrentSum}", this.CurrentSum));
        }

        protected internal void IncrementDays()
        {
            _days++;
        }
        // начисление процентов
        protected internal virtual void Calculate()
        {
            decimal increment = _sum * _percentage / 100;
            _sum = _sum + increment;
            OnCalculated(new AccountEventArgs($"Начислены проценты в размере: {increment}",increment));
        }

    }
}
