using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ViewModelBase
{
    public class ViewModelBase:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyOfPropertyChanged<TProperty>(Expression<Func<TProperty>> property)
        {
            var propertyExpression = property.Body as MemberExpression;
            PropertyInfo prop = propertyExpression.Member as PropertyInfo;
            NotifyPropertyChanged(prop.Name);
        }

        protected void NotifyPropertyChanged(string propertyName)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentNullException("propertyName");
            }

            if (!this.GetType().GetProperties().Any(x => x.Name == propertyName))
            {
                throw new ArgumentException(
                    "The property name does not exist in this type.",
                    "propertyName");
            }

            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }

        } 
    }
}
