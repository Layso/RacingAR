//------------------------------------------------------------------------------
// <auto-generated />
//
// This file was automatically generated by SWIG (http://www.swig.org).
// Version 4.0.0
//
// Do not make changes to this file unless you know what you are doing--modify
// the SWIG interface file instead.
//------------------------------------------------------------------------------

namespace Grapeshot {

public abstract class Driver : global::System.IDisposable {
  private global::System.Runtime.InteropServices.HandleRef swigCPtr;
  private bool swigCMemOwnBase;

  internal Driver(global::System.IntPtr cPtr, bool cMemoryOwn) {
    swigCMemOwnBase = cMemoryOwn;
    swigCPtr = new global::System.Runtime.InteropServices.HandleRef(this, cPtr);
  }

  internal static global::System.Runtime.InteropServices.HandleRef getCPtr(Driver obj) {
    return (obj == null) ? new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero) : obj.swigCPtr;
  }

  ~Driver() {
    Dispose();
  }

  public virtual void Dispose() {
    lock(this) {
      if (swigCPtr.Handle != global::System.IntPtr.Zero) {
        if (swigCMemOwnBase) {
          swigCMemOwnBase = false;
          GrapeshotDriverPINVOKE.delete_Driver(swigCPtr);
        }
        swigCPtr = new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero);
      }
      global::System.GC.SuppressFinalize(this);
    }
  }

  public virtual void process() {
    GrapeshotDriverPINVOKE.Driver_process(swigCPtr);
    if (GrapeshotDriverPINVOKE.SWIGPendingException.Pending) throw GrapeshotDriverPINVOKE.SWIGPendingException.Retrieve();
  }

  public virtual void sendChunk(System.Guid clientIdentifier, ror.schema.upload.UploadChunkRequest chunk, RequestCallback_FunctionApplicationBridgeBase callback) {
var clientIdentifier_Bytes = clientIdentifier.ToByteArray();var clientIdentifier_ByteArrayHandle = System.Runtime.InteropServices.GCHandle.Alloc(clientIdentifier_Bytes ,System.Runtime.InteropServices.GCHandleType.Pinned);var clientIdentifier_buffer_ptr = clientIdentifier_ByteArrayHandle.AddrOfPinnedObject();
var chunk_table = FlatBuffers.TableUtils.GetTable(chunk);var chunk_length = chunk_table.bb.Length;var chunk_pos = chunk_table.bb_pos;var chunk_seg = chunk_table.bb.ToArraySegment(chunk_pos, chunk_length - chunk_pos);var chunk_managed_array = chunk_seg.Array;var chunk_managed_handle = System.Runtime.InteropServices.GCHandle.Alloc(chunk_managed_array, System.Runtime.InteropServices.GCHandleType.Pinned);var chunk_buffer_ptr = chunk_managed_handle.AddrOfPinnedObject();System.IntPtr chunk_fb_ptr;unsafe {    var chunk_buffer_unsafe_ptr = (byte*) chunk_buffer_ptr.ToPointer();    chunk_fb_ptr = (System.IntPtr) (chunk_buffer_unsafe_ptr + chunk_pos);}var chunk_intermediate = new FBIntermediateBuffer();chunk_intermediate.ptr = chunk_fb_ptr;
callback.allocApplicationCallback();
    try {
      GrapeshotDriverPINVOKE.Driver_sendChunk(swigCPtr, clientIdentifier_buffer_ptr, chunk_intermediate, RequestCallback_FunctionApplicationBridgeBase.getCPtr(callback));
      if (GrapeshotDriverPINVOKE.SWIGPendingException.Pending) throw GrapeshotDriverPINVOKE.SWIGPendingException.Retrieve();
    } finally {
clientIdentifier_ByteArrayHandle.Free();
chunk_managed_handle.Free();
    }
  }

}

}
