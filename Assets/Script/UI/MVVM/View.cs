using UnityEngine;
using System;

namespace DemonCastle.UI
{
    public abstract class View<TViewModel> : MonoBehaviour where TViewModel : class
    {
        protected TViewModel ViewModel { get; private set; }

        public void Bind(TViewModel viewModel)
        {
            if (viewModel == null) throw new ArgumentNullException(nameof(viewModel));

            Unbind();
            ViewModel = viewModel;
            OnBind(viewModel);
        }
        
        public void Unbind()
        {
            if (ViewModel == null) return;
            OnUnbind(ViewModel);
            ViewModel = null;
        }

        protected abstract void OnBind(TViewModel viewModel);
        protected abstract void OnUnbind(TViewModel viewModel);

        protected virtual void OnDestroy()
        {
            Unbind();
        }
    }
}

