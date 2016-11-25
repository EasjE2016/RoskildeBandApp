using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;

namespace RoskildeBandApp
{
    /// <summary>
    /// A command whose sole purpose is to relay its functionality 
    /// to other objects by invoking delegates. 
    /// The default return value for the CanExecute method is 'true'.
    /// <see cref="RaiseCanExecuteChanged"/> needs to be called whenever
    /// <see cref="CanExecute"/> is expected to return a different value.
    /// </summary>
    public class RelayCommand : ICommand
    {
        private Action methodToExecute;
        private Func<bool> methodToDetectCanExecute;

        private DispatcherTimer canExecuteChangedEventTimer = null;
        
        /// <summary>
        /// Raised when RaiseCanExecuteChanged is called.
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Creates a new command that can always execute.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        public RelayCommand(Action execute)
            : this(execute, null)
        {
        }


        /// <summary>
        /// Creates a new command.
        /// </summary>
        /// <param name="methodToExecute">The execution logic.</param>
        /// <param name="methodToDetectCanExecute">The execution status logic.</param>
        public RelayCommand(Action methodToExecute, Func<bool> methodToDetectCanExecute)
        {
            if (methodToExecute == null)
                throw new ArgumentNullException("execute");

            this.methodToExecute = methodToExecute;
            this.methodToDetectCanExecute = methodToDetectCanExecute;
            if(methodToDetectCanExecute != null)
            {
                this.canExecuteChangedEventTimer.Tick += canExecuteChangedEventTimer_Tick;
                this.canExecuteChangedEventTimer.Interval = new TimeSpan(0, 0, 1);
                this.canExecuteChangedEventTimer.Start();
            }
        }

        /// <summary>
        /// Determines whether this <see cref="RelayCommand"/> can execute in its current state.
        /// </summary>
        /// <param name="parameter">
        /// Data used by the command. If the command does not require data to be passed, this object can be set to null.
        /// </param>
        /// <returns>true if this command can be executed; otherwise, false.</returns>
        public bool CanExecute(object parameter)
        {
            //return true;
            return methodToDetectCanExecute == null ? true : methodToDetectCanExecute();
        }

        /// <summary>
        /// Executes the <see cref="RelayCommand"/> on the current command target.
        /// </summary>
        /// <param name="parameter">
        /// Data used by the command. If the command does not require data to be passed, this object can be set to null.
        /// </param>
        public void Execute(object parameter)
        {
            methodToExecute();
        }

        void canExecuteChangedEventTimer_Tick(object sender, object e)
        {
            if (this.CanExecuteChanged != null)
            {
                this.CanExecuteChanged(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Method used to raise the <see cref="CanExecuteChanged"/> event
        /// to indicate that the return value of the <see cref="CanExecute"/>
        /// method has changed.
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            var handler = CanExecuteChanged;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }
    }
}

