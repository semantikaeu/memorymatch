namespace Assets.GameEngine
{
    using System.Collections.Generic;
    using System.ComponentModel;

    /// <summary>
    /// This class simplifies implementation of INotifyPropertyChanged.
    /// </summary>
    public class ObservableObject : INotifyPropertyChanged
    {
        /// <summary>
        /// Occurs when property has changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Called when property has changed.
        /// </summary>
        /// <param name="propertyName">Name of changed property.</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// Sets and notifies property if property has changed changed.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="field">The field.</param>
        /// <param name="newValue">The new value.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>Returns if value has changed.</returns>
        protected bool SetProperty<T>(ref T field, T newValue, string propertyName)
        {
            if (EqualityComparer<T>.Default.Equals(field, newValue))
            {
                return false;
            }

            field = newValue;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
