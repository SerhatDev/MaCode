## **What is MaCode ?**

MaCode includes tools to help developers for every projects routines. More tools are added with time.

### **SweetAlerts2**

Wrapper for [SweetAlerts2](https://sweetalert2.github.io/) javascript library.

  #### Usage
  
  Inject ISweetlAlerts2 interface in your .cshtml file and initialize the plugin.
  
  ```csharp
  @inject MaCode.Plugins.ISweetAlerts2 Swal
  
  @Swal.UseSwal()
  ```
  'UseSwal' method will reference necessary js/css files.
  
  #### Show Alert
  Use 'Success' method to create a success alert.
  ```csharp
  @Swal.Success("Done!", "Good")
  ```
  Use 'Success' method to create a error alert.
  ```csharp
  @Swal.Error("Done!", "Good")
  ```
  Use 'Dialog' method to create a error alert.
  ```csharp
  @Swal.Dialog("Title", "Are you sure ?")
  ```
  #### Additional Parameters
  
| Parameter Name | Values | Default | Description |
| -------------  | ------ | ------- | ----------- |
| icon   | **string**| **warning, error, success, info, question** | set icon|
| addScriptTag   | **true**,**false**|**false**| if you are calling plugin in script tag set this to false otherwise set it to true |
| showButton   | **true**,**false**|**false**| show default button |
| buttonText   | **string**| | set default button value|
| toast   | **true**,**false**|**false** | show alert as toast |
| toastTimeout   | **int**|**1500** | set the timeout for toast message in ms |
| toastPosition   | **top**, **top-start**, **top-end**, **center**, **center-start**, **center-end**, **bottom**, **bottom-start**,  **bottom-end**|**top-end** | set position of toast message |
| timer   | **int**|**0** | set the timeout for alert box. Use this if it's not toast message |
| when   | **bool**|**true** | set condition when to show the alert |
| afterClosed   | **string**,**SwalObject**|**null** | define callback for alert closed event. You can pass plain javascript as string or create another SweetAlerts alert here |

  #### Dialog Specific Parameters
| Parameter Name | Values | Default | Description |
| -------------  | ------ | ------- | ----------- |
| showCancelButton  | **true,false** | **true** | display cancel button in dialog |
| cancelButtonText  | **string** | **Cancel** | set the text of the cancel button |
| cancelButtonColor  | **string** | **#d33** | color of cancel button |
| confirmButtonText  | **string** | **Yes** | set the text of the confirm button |
| confirmButtonColor  | **string** | **#3085d6** | color of confirm button |
| successCallback  | **string** | **null** | set success callback |
| cancelCallback  | **string** | **null** | set cancel callback |

 #### Usage Examples
 
  ```csharp
  // Use SweetAlerts object as callback
  @Swal.Success("Title","Message",
        afterClosed:Swal.Success("Callback title","callback message"))
        
  // Show toast alert with javascript string as callback
  @Swal.Success("Title","Message",
        toast:true,
        toastTimeout:3000,
        afterClosed:"window.location.href='/'")
        
  // Dialog with callbacks
  @Swal.Dialog("Question","Are you sure ?"
        ,confirmButtonText:"Do it!"
        ,cancelButtonText:"Nope!"
        ,successCallback:"Done()"
        ,cancelCallback:"Cancel()")
        
   // Show alert with condition
   @Swal.Success("Title","Message",when:Model.hasMessage)
  ```
  
