// Decompiled with JetBrains decompiler
// Type: PCLStorage.AwaitExtensions
// Assembly: PCLStorage, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: FB422C97-CC83-4BAE-A6A4-C42C2408B899
// Assembly location: C:\Users\pc\reverse_engineering\com.companyname.Shelf\assemblies\PCLStorage.dll

using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace PCLStorage
{
  internal static class AwaitExtensions
  {
    internal static AwaitExtensions.TaskSchedulerAwaiter SwitchOffMainThreadAsync(
      CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();
      return new AwaitExtensions.TaskSchedulerAwaiter(SynchronizationContext.Current != null ? TaskScheduler.Default : (TaskScheduler) null, cancellationToken);
    }

    internal struct TaskSchedulerAwaiter : INotifyCompletion
    {
      private TaskScheduler taskScheduler;
      private CancellationToken cancellationToken;

      internal TaskSchedulerAwaiter(
        TaskScheduler taskScheduler,
        CancellationToken cancellationToken)
      {
        this.taskScheduler = taskScheduler;
        this.cancellationToken = cancellationToken;
      }

      internal AwaitExtensions.TaskSchedulerAwaiter GetAwaiter() => this;

      public bool IsCompleted => this.taskScheduler == null;

      public void OnCompleted(Action continuation)
      {
        if (this.taskScheduler == null)
          throw new InvalidOperationException("IsCompleted is true, so this is unexpected.");
        Task.Factory.StartNew(continuation, CancellationToken.None, TaskCreationOptions.None, this.taskScheduler);
      }

      public void GetResult() => this.cancellationToken.ThrowIfCancellationRequested();
    }
  }
}
