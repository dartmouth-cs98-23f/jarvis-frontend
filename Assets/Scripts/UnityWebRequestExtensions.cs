using System.Runtime.CompilerServices;
using UnityEngine.Networking;

public static class UnityWebRequestExtensions
{
    public struct UnityWebRequestAwaiter : INotifyCompletion
    {
        private UnityWebRequestAsyncOperation asyncOp;
        public UnityWebRequestAwaiter(UnityWebRequestAsyncOperation asyncOp) { this.asyncOp = asyncOp; }
        public bool IsCompleted => asyncOp.isDone;
        public void OnCompleted(System.Action continuation) => asyncOp.completed += _ => continuation();
        public void GetResult() { }
    }

    public static UnityWebRequestAwaiter GetAwaiter(this UnityWebRequestAsyncOperation asyncOp)
    {
        return new UnityWebRequestAwaiter(asyncOp);
    }
}