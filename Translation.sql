
/*<History Author='Nabeel ur Rehman' Date='26-01-2023'> Example of Create Procedure </History>*/

EXEC [dbo].pInsTranslation 'Legislation_Template_Setup',N'إعداد قالب التشريع','Template Setup','Legal Legislation List View Page',1
EXEC [dbo].pInsTranslation 'Preview_Draft',N'معاينة المسودة','Preview Draft','Case Template',1
EXEC [dbo].pInsTranslation 'Preview_And_Submit',N'معاينة وإرسال','Preview & Submit','Meeting List',1
EXEC [dbo].pInsTranslation 'Opening_Statement',N'البيان الافتتاحي','Opening Statement','Case Template',1
EXEC [dbo].pInsTranslation 'File_Draft',N'مسودة الملف','File Draft','Case Template',1
EXEC [dbo].pInsTranslation 'Draft_Without_Template',N'مسودة بدون قالب','Draft Without Template','Case Template',1
EXEC [dbo].pInsTranslation 'SendAcopy',N'أرسل نسخة','Send A Copy','Send Copy',1
EXEC [dbo].pInsTranslation 'Individual',N'فردي','Individual','Case Request Detail',1
EXEC [dbo].pInsTranslation 'Add_Case_Request_Success',N'تم إنشاء #entity# بنجاح في النظام. يرجى زيارة الرابط المقدم لمزيد من التفاصيل .',' #entity# is successfully created in the system. Please visit provided link for more details.','Notification Module',1
EXEC [dbo].pInsTranslation 'Individual',N'فردي','Individual','Case Request Detail',1

-----11/02/2023	
EXEC [dbo].pInsTranslation 'Required_Field',N'هذه الخانة مطلوبه.','This field is required.','General',1
EXEC [dbo].pInsTranslation 'Serial_Number',N'رقم التسلسل','Serial No.','Meeting',1
EXEC [dbo].pInsTranslation 'Request_From',N'طلب من','Request From','Case Request',1
EXEC [dbo].pInsTranslation 'Request_To',N'طلب إلى','Request_To','Case Request',1
EXEC [dbo].pInsTranslation 'Serial_Number',N'رقم التسلسل','Serial No.','Meeting',1
EXEC [dbo].pInsTranslation 'Civil_Id_No',N'رقم البطاقة المدنية','Civil ID No.','Case File',1
EXEC [dbo].pInsTranslation 'Must_Four_Characters',N'Must Type Four Characters','Must Type Four Characters','Case File',1
EXEC [dbo].pInsTranslation 'Civil_Id_No',N'رقم البطاقة المدنية','Civil ID No.','Case File',1
EXEC [dbo].pInsTranslation 'Continue',N'متابعة','Continue','General',1

EXEC [dbo].pInsTranslation 'Items_Per_Page',N'سجل لكل صفحة','Items per page','General',1
EXEC [dbo].pInsTranslation 'Is_null',N'باطل','Is Null','General',1
EXEC [dbo].pInsTranslation 'Page',N'صفحة','Page','General',1
EXEC [dbo].pInsTranslation 'of',N'من','of','General',1
EXEC [dbo].pInsTranslation 'records',N'سجل','records','General',1
EXEC [dbo].pInsTranslation 'items',N'سجل','items','General',1


-----01/01/2023	
EXEC [dbo].pInsTranslation 'items',N'سجل','items','General',1
---------consultation
EXEC [dbo].pInsTranslation 'Request_Type',N'نوع الطلب','Request Type','Consultation Request Detail',1
EXEC [dbo].pInsTranslation 'Request_Date',N'تاريخ الطلب','Request Date','Consultation Request List',1
EXEC [dbo].pInsTranslation 'Request_Title',N' عنوان الطلب','Request Title','Consultation Request List',1
EXEC [dbo].pInsTranslation 'Type_Of_Contract',N'نوع العقد','Type Of Contract','Consultation Request List',1
EXEC [dbo].pInsTranslation 'Contract_Amount_75000KD',N'مبلغ العقد 75000 دينار كويتي','Contract Amount 75000KD','Consultation Request List',1
EXEC [dbo].pInsTranslation 'Govermemt_Opinion',N'رأي الحكومة','Goverment Opinion','Consultation Request List',1
EXEC [dbo].pInsTranslation 'Parties',N'الأطراف','Parties','Consultation Request List',1
EXEC [dbo].pInsTranslation 'Introduction',N'المقدمة','Introduction','Consultation Request List',1
EXEC [dbo].pInsTranslation 'Article',N'مادة','Article','Consultation Request List',1
EXEC [dbo].pInsTranslation 'List_Documents',N'قائمة الملفات','List of Documents','Consultation Request List',1
EXEC [dbo].pInsTranslation 'Activity',N'نشاط','Activity','Consultation Request List',1
EXEC [dbo].pInsTranslation 'InboxNo',N'رقم الصندوق الوارد ','Inbox No','Consultation Request List',1
EXEC [dbo].pInsTranslation 'InboxDate',N'تاريخ الوارد','Inbox Date','Consultation Request List',1
EXEC [dbo].pInsTranslation 'OutboxDate',N'تاريخ الصادر','Outbox Date','Consultation Request List',1
EXEC [dbo].pInsTranslation 'OutboxNo',N'رقم الصادر','Outbox No','Consultation Request List',1
EXEC [dbo].pInsTranslation 'Communication',N'تواصل','Communication','Consultation Request List',1
EXEC [dbo].pInsTranslation 'Initiated_By',N'بدأها','Initiated By','Consultation Request List',1
EXEC [dbo].pInsTranslation 'Correspondence_Type',N'نوع المراسلات','Correspondence Type','Consultation Request List',1
EXEC [dbo].pInsTranslation 'Reamrks',N'ملاحظات','Reamrks','Consultation Request List',1
EXEC [dbo].pInsTranslation 'Transfer',N'تحويل','Transfer','Consultation Request List',1
EXEC [dbo].pInsTranslation 'Assign_To_Lawyer',N'تعيين للمحامي','Assign To Lawyer','Consultation Request List',1
EXEC [dbo].pInsTranslation 'Request_More_Info',N'طلب المزيد من المعلومات','Request More Info','Consultation Request List',1
EXEC [dbo].pInsTranslation 'History_Details',N'تفاصيل التغيرات','History Details','Consultation Request List',1

EXEC [dbo].pInsTranslation 'Party_Type',N'نوع الطرف','Party Type','Consultation Request Detail',1
EXEC [dbo].pInsTranslation 'Consultation_Requests_Detail',N' تفاصيل طلبات الاستشارة','Consultation Requests Detail','Consultation Request Detail',1
EXEC [dbo].pInsTranslation 'History_Of_Consultation_Request',N'طلب تاريخ الاستشارة','History Of Consultation Request','Consultation Request Detail',1
EXEC [dbo].pInsTranslation 'Consultation_Info',N'تفاصيل الاستشارة','Consultation Information','Consultation Request Detail',1
EXEC [dbo].pInsTranslation 'Type_Request',N'نوع الطلب','Request Type','Consultation Request List',1
EXEC [dbo].pInsTranslation 'Consultation_File',N'ملف الاستشارة','Consultation File','Consultation File List',1
EXEC [dbo].pInsTranslation 'Lawyer',N' محامي ','Lawyer','Consultation File List',1
EXEC [dbo].pInsTranslation 'Assignment',N'تكليف','Assignment','Consultation File List',1
EXEC [dbo].pInsTranslation 'Consultation_File_Details',N'تفاصيل ملف الاستشارة','Consultation File Detail','Consultation File Detail',1
EXEC [dbo].pInsTranslation 'Consultation_File_Info',N'معلومات ملف الاستشارة','Consultation File Info','Consultation File Detail',1
-----------------
EXEC [dbo].pInsTranslation 'Reason_Section',N'قسم السبب','Reason Section','Draft Detail',1
EXEC [dbo].pInsTranslation 'Inbox_Outbox',N'المراسلات الواردة / الصادرة','Inbox / Outbox','COMM',1
EXEC [dbo].pInsTranslation 'Consultation_Request_List',N'قائمة طلبات الاستشاري','Consultation Request List','Consultation',1
EXEC [dbo].pInsTranslation 'Pending_Requests',N'الطلبات المعلقة','Pending Requests','CMS',1
EXEC [dbo].pInsTranslation 'Consultation_Request',N'طلب الاستشاري','Consultation Request','Consultation Menu',1
EXEC [dbo].pInsTranslation 'Request_Details',N'تفاصيل الطلب','Request Details','Consultation Request List',1
EXEC [dbo].pInsTranslation 'Request_Status',N'حالة الطلب','Request Status','Consultation Request Detail',1
EXEC [dbo].pInsTranslation 'Goverment_Entity',N'الجهة الحكومية','Government Entity','G2G Request Detail  ',1
EXEC [dbo].pInsTranslation 'Sector',N'قطاع','Sector','Case File  ',1
EXEC [dbo].pInsTranslation 'Required_Field_Reasons',N'الحقل المطلوب','Reason Field Is Required','case view detail ',1
EXEC [dbo].pInsTranslation 'Consultation_Files',N'ملفات الاستشاري','Consultation Files','Consultation Side Menu',1
EXEC [dbo].pInsTranslation 'Add_Draft',N'إضافة مسودة','Add Draft','Case File',1
EXEC [dbo].pInsTranslation 'Select_Draft_Type',N'اختر نوع المسودة','Select Draft Type','Case Template',1
EXEC [dbo].pInsTranslation 'Draft_From_Template',N'مسودة من القالب','Draft From Template','Case Template',1
EXEC [dbo].pInsTranslation 'Document_Name',N'اسم المستند','Document Name','Case File',1
EXEC [dbo].pInsTranslation 'Created_Datetime',N'وقت وتاريخ الانشاء','Created Datetime','Case Template',1
EXEC [dbo].pInsTranslation 'Last_Modified_Datetime',N'وقت وتاريخ آخر تعديل','Last Modified Datetime','Case Template',1
EXEC [dbo].pInsTranslation 'Select_Templates',N'اختر قالب','Select Template','Case Template',1
EXEC [dbo].pInsTranslation 'Add_Article',N'أضف مادة','Add Article','New Consultation Request Add Page',1
EXEC [dbo].pInsTranslation 'Article_Status',N'حالة المادة','Article Status','New Consultation Request Add Page',1
EXEC [dbo].pInsTranslation 'Article_Name',N'اسم المادة','Article Name','New Consultation Request Add Page',1
EXEC [dbo].pInsTranslation 'Article_Parent_Section',N'القسم الرئيسي للمادة','Article Parent Section','New Consultation Request Add Page',1
EXEC [dbo].pInsTranslation 'Consultation_Introduction',N'المقدمة','Introduction','Add Consultation Request Form Page',1
EXEC [dbo].pInsTranslation 'Legislations',N'التشريعات','Legislations','Consultation trasnfer',1
EXEC [dbo].pInsTranslation 'Attachments',N'المرفقات','Attachments','Request Need More Details Page',1
EXEC [dbo].pInsTranslation 'Submits',N'إرسال','Submit','Request Need More Details Page',1
EXEC [dbo].pInsTranslation 'Submit_Draft',N'إرسال مسودة','Submit Draft','Request Need More Details Page',1
EXEC [dbo].pInsTranslation 'Sure_Submit_Draft',N'هل أنت متأكد من رغبتك في تقديم مسودة؟','Are you sure you want to submit draft?','Request Need More Details Page',1
EXEC [dbo].pInsTranslation 'Reminder_Frequency',N'تكرار التذكير','Reminder Frequency','Case File',1
EXEC [dbo].pInsTranslation 'Additional_GE',N'جهة حكومية إضافية','Additional GE','Case File',1
EXEC [dbo].pInsTranslation 'Due_Date',N'تاريخ الاستحقاق','Due Date','Case File',1
EXEC [dbo].pInsTranslation 'Other',N'أخرى','Other','RequestForMoreInfo',1
EXEC [dbo].pInsTranslation 'Reason_Of_Response',N'سبب الرد','Reason of Response','RequestForMoreInfo',1
EXEC [dbo].pInsTranslation 'IsUrgent',N'عاجل؟','Is Urgent?','Case File',1
EXEC [dbo].pInsTranslation 'Inbox_Outbox',N'المراسلات الواردة / الصادرة','Inbox / Outbox','COMM',1
EXEC [dbo].pInsTranslation 'Sent_by',N'Sent By','Sent By','Tasks List Screen',1

---------
EXEC [dbo].pInsTranslation 'Contact_List',N'قائمة جهات الإتصال','Contact List','Contact Managment',1
EXEC [dbo].pInsTranslation 'Job_Role',N'الصفة','Job Role','Contact Managment',1
EXEC [dbo].pInsTranslation 'First_Name',N'الاسم الأول','First Name','Contact Managment',1
EXEC [dbo].pInsTranslation 'Second Name',N'الاسم الثاني','Second Name','Contact Managment',1
EXEC [dbo].pInsTranslation 'Last Name',N'الاسم الأخير','Last Name','Contact Managment',1
EXEC [dbo].pInsTranslation 'Contact_Details',N'بيانات جهة الاتصال','Contact Details','Contact Managment',1
EXEC [dbo].pInsTranslation 'Case_Details',N'تفاصيل القضية','Case Details','Contact Managment',1
EXEC [dbo].pInsTranslation 'phone_Number',N'رقم الهاتف المحمول','Mobile Number','Contact Managment',1
EXEC [dbo].pInsTranslation 'Linked_File_Details',N'تفاصيل الملف المرتبط','Linked File Details','Contact Managment',1
EXEC [dbo].pInsTranslation 'Linked_Request_Details',N'رقم الهاتف المحمول','Linked Request Details','Contact Managment',1
EXEC [dbo].pInsTranslation 'Claim_Amount',N'مبلغ القضية','Claim Amount','General',1
EXEC [dbo].pInsTranslation 'Case_Requirements',N'ملخص الطلبات','Case Requirements','General',1
 EXEC [dbo].pInsTranslation 'Decision_Request_List',N'قائمة طلبات القرار','Decision Request List','Decision Request',1
 EXEC [dbo].pInsTranslation 'GE_Decision_Requests',N'GE Decision Requests','GE Decision Requests','Decision Request',1
 EXEC [dbo].pInsTranslation 'Current_Tasks',N'المهام الحالية','Current Tasks','Assign to Lawyer Screen',1
 EXEC [dbo].pInsTranslation 'Last_Activity_Date',N'تاريخ آخر مهمة','Last Activity Date','Assign to Lawyer Screen',1
 EXEC [dbo].pInsTranslation 'Request_Assigned',N'تم تعيين الطلب','Request has been assigned','AssignToLawyer',1
---------------------
EXEC [dbo].pInsTranslation 'Case_Requirements',N'ملخص الطلبات','Case Requirements','General',1

-----------  Contact Management


EXEC [dbo].pInsTranslation 'Contact_Management',N'إدارة جهات الاتصال','Contact Management','New Contact Add Page',1
EXEC [dbo].pInsTranslation 'Contact_List',N'قائمة جهات الاتصال','Contact List','New Contact Add Page',1
EXEC [dbo].pInsTranslation 'Add_Contact',N'أضف جهة اتصال','Add Contact','New Contact Add Page',1
EXEC [dbo].pInsTranslation 'Edit_Contact',N'تعديل جهة اتصال','Edit Contact','New Contact Add Page',1
EXEC [dbo].pInsTranslation 'Contact_Type',N'النوع','Contact Type','New Contact Add Page',1
EXEC [dbo].pInsTranslation 'Contact_Role',N'دور جهة الاتصال','Contact Role','New Contact Add Page',1
EXEC [dbo].pInsTranslation 'Contact_Sector',N'القطاع','Contact Sector','New Contact Add Page',1
EXEC [dbo].pInsTranslation 'Contact_Department',N'القسم','Contact Department','New Contact Add Page',1
EXEC [dbo].pInsTranslation 'Contact_First_Name',N'الاسم الأول','First Name','New Contact Add Page',1
EXEC [dbo].pInsTranslation 'Contact_Second_Name',N'الاسم الثاني','Second Name','New Contact Add Page',1
EXEC [dbo].pInsTranslation 'Contact_Last_Name',N'الاسم الأخير','Last Name','New Contact Add Page',1
EXEC [dbo].pInsTranslation 'Contact_Email',N'البريد الإلكتروني','Contact Email','New Contact Add Page',1
EXEC [dbo].pInsTranslation 'Contact_Note',N'ملاحظات','Contact Note','New Contact Add Page',1
EXEC [dbo].pInsTranslation 'Contact_Phone',N'رقم الهاتف','Contact Phone Number','New Contact Add Page',1
EXEC [dbo].pInsTranslation 'Link_Documents',N'الملفات المرتبطة','Link Documents','New Contact Add Page',1
EXEC [dbo].pInsTranslation 'Link_Case',N'القضايا المرتبطة','Link Cases','New Contact Add Page',1
EXEC [dbo].pInsTranslation 'Add_Link_File',N'إضافة الملف المرتبط','Add Link File','New Contact Add Page',1
EXEC [dbo].pInsTranslation 'Add_Link_Request',N'إضافة الطلب المرتبط','Add Link Request','New Contact Add Page',1
EXEC [dbo].pInsTranslation 'Add_Link_Transaction_Reference',N'إضافة مرجع العمليات المرتبطة','Add Link Transaction Reference','New Contact Add Page',1
EXEC [dbo].pInsTranslation 'Add_Contact_Link_Document',N'ربط المستند','Link Document','New Contact Add Page',1
EXEC [dbo].pInsTranslation 'Select_File',N'اختيار الملف','Select File','New Contact Add Page',1
EXEC [dbo].pInsTranslation 'Select_Request',N'اختيار الطلب','Select Request','New Contact Add Page',1
EXEC [dbo].pInsTranslation 'Add_File',N'إضافة ملف','Add File','New Contact Add Page',1
EXEC [dbo].pInsTranslation 'Add_Request',N'إضافة الطلب','Add Request','New Contact Add Page',1
EXEC [dbo].pInsTranslation 'File_Request_Number',N'رقم الملف/الطلب','File Request Number','New Contact Add Page',1
EXEC [dbo].pInsTranslation 'File_Request_Name',N'اسم الملف/الطلب','File/Request Name','New Contact Add Page',1
EXEC [dbo].pInsTranslation 'Delete_File_Request_Grid_Confirm_Message',N'هل أنت متأكد أنك تريد حذف هذا السجل؟','Are you sure you want to delete record?','New Contact Add Page',1
EXEC [dbo].pInsTranslation 'Delete_File_Request_Success_Message',N'تم حذف السجل بنجاح','Record deleted successfully.','New Contact Add Page',1
EXEC [dbo].pInsTranslation 'Add_Contact_Success',N'تم إضافة جهة اتصال جديدة بنجاح','New contact is successfully added in the system.','New Contact Add Page',1

EXEC [dbo].pInsTranslation 'Consultation_Submit',N'هل أنت متأكد أنك تريد حفظ جهة الاتصال؟','Are you sure you want to save contact?','New Contact Add Page',1
EXEC [dbo].pInsTranslation 'Consultation_Submit_Successfully',N'تم إضافة جهة اتصال جديدة بنجاح','New contact is successfully added in the system.','New Contact Add Page',1
EXEC [dbo].pInsTranslation 'Mandatory_Documents',N'المستندات المطلوبة','Mandatory Documents','New Contact Add Page',1
EXEC [dbo].pInsTranslation 'Additional_Documents',N'المستندات الإضافية','Additional Documents','New Contact Add Page',1
EXEC [dbo].pInsTranslation 'Upload_Attachment',N'تحميل المرفق','Upload Attachment','New Contact Add Page',1
EXEC [dbo].pInsTranslation 'Nothing_At_This_Address',N'عذرا، لا يوجد شيء في هذا العنوان','Sorry, there is nothing at this address.','General',1
EXEC [dbo].pInsTranslation 'Select_More_Member',N'اختيار أكثر من عضو','Select more than one member','AssignToLawyer',1


EXEC [dbo].pInsTranslation 'List_Legal_Advice_File',N'الفتوى','Legal Advice','List Consultation File Page',1
EXEC [dbo].pInsTranslation 'List_Contract_File',N'العقود','Contracts','List Consultation File Page',1
EXEC [dbo].pInsTranslation 'List_Legislations_File',N'التشريع','Legislations','List Consultation File Page',1
EXEC [dbo].pInsTranslation 'Administrative_Complaints_File',N'التظلمات الإدارية','Administrative Complaints','List Consultation File Page',1
EXEC [dbo].pInsTranslation 'List_International_Arbitration_File',N'التحكيم الدولي','International Arbitration','List Consultation File Page',1
EXEC [dbo].pInsTranslation 'Contracts_File_Details',N'تفاصيل ملف العقود','Contracts File Detail','List Consultation File Page',1
EXEC [dbo].pInsTranslation 'Legisltation_File_Details',N'تفاصيل ملف التشريع','Legislations File Detail','List Consultation File Page',1
EXEC [dbo].pInsTranslation 'Administrative_Complaints_File_Details',N'تفاصيل ملف التظلمات الإدارية','Administrative Complaints File Detail','List Consultation File Page',1
EXEC [dbo].pInsTranslation 'Legal_Advice_File_Details',N'تفاصيل ملف التظلمات الإداريةتفاصيل ملف الفتوى','Legal Advice File Detail','List Consultation File Page',1
EXEC [dbo].pInsTranslation 'International_Arbitration_File_Details',N'تفاصيل ملف التحكيم الدولي','International Arbitration File Detail','List Consultation File Page',1

EXEC [dbo].pInsTranslation 'List_Legal_Advice_File',N'ملفات الفتوى','Legal Advice Files','List Consultation File Page',1
EXEC [dbo].pInsTranslation 'List_Contract_File',N'ملفات العقود','Contract Files','List Consultation File Page',1
EXEC [dbo].pInsTranslation 'List_Legislations_File',N'ملفات التشريع','Legislation Files','List Consultation File Page',1
EXEC [dbo].pInsTranslation 'Administrative_Complaints_File',N'ملفات التظلمات الإدارية','Administrative Complaint Files','List Consultation File Page',1
EXEC [dbo].pInsTranslation 'List_International_Arbitration_File',N'ملفات التحكيم الدولي','International Arbiteration Files','List Consultation File Page',1

EXEC [dbo].pInsTranslation 'Contracts_File_Details',N'تفاصيل ملف العقود','Contracts File Detail','List Consultation File Page',1
EXEC [dbo].pInsTranslation 'Legisltation_File_Details',N'تفاصيل ملف التشريع','Legislations File Detail','List Consultation File Page',1
EXEC [dbo].pInsTranslation 'Administrative_Complaints_File_Details',N'تفاصيل ملف التظلمات الإدارية','Administrative Complaints File Detail','List Consultation File Page',1
EXEC [dbo].pInsTranslation 'Legal_Advice_File_Details',N'تفاصيل ملف التظلمات الإداريةتفاصيل ملف الفتوى','Legal Advice File Detail','List Consultation File Page',1
EXEC [dbo].pInsTranslation 'International_Arbitration_File_Details',N'تفاصيل ملف التحكيم الدولي','International Arbitration File Detail','List Consultation File Page',1

EXEC [dbo].pInsTranslation 'Legal_Advice_File',N'ملفات الفتوى','Legal Advice Files','List Consultation File Page',1
EXEC [dbo].pInsTranslation 'Contracts_File',N'ملفات العقود','Contract Files','List Consultation File Page',1
EXEC [dbo].pInsTranslation 'List_Legislations_File',N'ملفات التشريع','Legislation Files','List Consultation File Page',1
EXEC [dbo].pInsTranslation 'Administrative_Complaints_File',N'ملفات التظلمات الإدارية','Administrative Complaint Files','List Consultation File Page',1
EXEC [dbo].pInsTranslation 'International_Arbitration_File',N'ملفات التحكيم الدولي','International Arbiteration Files','List Consultation File Page',1


EXEC [dbo].pInsTranslation 'Contracts_Request_List',N'طلبات العقود','Contract Request','List Consultation File Page',1
EXEC [dbo].pInsTranslation 'International_Arbitration_Request_List',N'طلبات التحكيم الدولي','International Arbiteration Request','List Consultation File Page',1
EXEC [dbo].pInsTranslation 'Legal_Advice_Request_List',N'طلبات الفتوى','Legal Advice Request','List Consultation File Page',1
EXEC [dbo].pInsTranslation 'Legislations_Request_List',N'طلبات التشريع','Legislation Request','List Consultation File Page',1
EXEC [dbo].pInsTranslation 'Administrative_Complaints_Request_List',N'طلبات التظلمات الإدارية','Administrative Complaint Request','List Consultation File Page',1

EXEC [dbo].pInsTranslation 'Contracts_Request',N'طلبات العقود','Contract Request','List Consultation File Page',1
EXEC [dbo].pInsTranslation 'International_Arbitration_Request',N'طلبات التحكيم الدولي','International Arbiteration Request','List Consultation File Page',1
EXEC [dbo].pInsTranslation 'Legal_Advice_Request',N'طلبات الفتوى','Legal Advice Request','List Consultation File Page',1
EXEC [dbo].pInsTranslation 'Legislations_Request',N'طلبات التشريع','Legislation Request','List Consultation File Page',1
EXEC [dbo].pInsTranslation 'Administrative_Complaints_Request',N'طلبات التظلمات الإدارية','Administrative Complaint Request','List Consultation File Page',1



EXEC [dbo].pInsTranslation 'Contracts_Requests_Detail',N'تفاصيل طلب العقد','Contract Request detail','List Consultation File Page',1
EXEC [dbo].pInsTranslation 'Legal_Advice_Requests_Detail',N'تفاصيل طلب الفتوى','Legal Advice Request Detail','List Consultation File Page',1
EXEC [dbo].pInsTranslation 'International_Arbitration_Requests_Detail',N'تفاصيل طلب التحكيم الدولي ','International Arbiteration Request detail','List Consultation File Page',1
EXEC [dbo].pInsTranslation 'Administrative_Complaints_Requests_Detail',N'تفاصيل طلب التظلمات الإدارية ','Administrative Complaint Request Detail','List Consultation File Page',1
EXEC [dbo].pInsTranslation 'Legislations_Requests_Detail',N' تفاصيل طلب التشريع ','Legislation Request Detail','List Consultation File Page',1


EXEC [dbo].pInsTranslation 'Fatwa_Priority',N'أولوية الفتوى','Fatwa Priority','List Consultation File Page',1
--------------- 18/4/2023 -------------------------------
EXEC [dbo].pInsTranslation 'Floor_Number',N'Floor Number','Floor Number','Register Case page',1
EXEC [dbo].pInsTranslation 'Room_Number',N'Room_Number','Room_Number','Register Case page',1
EXEC [dbo].pInsTranslation 'Announcement_Number',N'Announcement Number','Announcement Number','Register Case page',1
EXEC [dbo].pInsTranslation 'Case_Open_Date',N'Case Open Date','Case Open Date','Register Case page',1


EXEC [dbo].pInsTranslation 'Assign_Back_Hos',N'Assign Back To HOS','Assign Back To HOS','Assign Back To HOS Button',1
 
EXEC [dbo].pInsTranslation 'Case_File_Assign_To_Hos',N'Case File Assign Successfully To HOS','Case File Assign Successfully To HOS','Success Message',1
EXEC [dbo].pInsTranslation 'Add_Assigned_Back_To_Hos_Success',N'#entity# Case File has assign Back To HOS Successfully. Please visit provided link for more details.',' #entity# Case File  has assign Back To HOS Successfully. Please visit provided link for more details.','Notification Module',1

------------------Transltation   FATWA_DB
EXEC [dbo].pInsTranslation 'Request_For_More_Information',N'طلب المزيد من المعلومات','Request for More Information','Draft Detail',1
EXEC [dbo].pInsTranslation 'Create_Contract_Request',N'إنشاء طلب العقود ','Create Contract Request','Communication',1
EXEC [dbo].pInsTranslation 'Create_legislation_Request',N'إنشاء طلب التشريعات  ','Create legislation Request  ','Communication',1
EXEC [dbo].pInsTranslation 'Create_Legal_Advice_Request',N'إنشاء طلب الفتوى','Create Legal Advice Request','Communication',1
EXEC [dbo].pInsTranslation 'Create_Administrative_Complaints_Request',N'إنشاء طلب التظلم الإداري ','Create Administrative Complaints Request','Communication',1
EXEC [dbo].pInsTranslation 'Create_International_Arbitration_Request',N'إنشاء طلب التحكيم الدولي ','Create International Arbitration Request','Communication',1
EXEC [dbo].pInsTranslation 'Schedule_Meeting',N'طلب اجتماع','Schedule Meeting','Meeting Add',1
EXEC [dbo].pInsTranslation 'Consultation_Tasks',N'مهام الاستشاري','Consultation Tasks','Task List',1
-------------------------------fatwa
EXEC [dbo].pInsTranslation 'International_Arbitration_Type',N'نوع التحكيم الدولي','International Arbitration Type','New Consultation Request Add Page',1
EXEC [dbo].pInsTranslation 'Complaints_Against',N'اسم المتظلم منه','Complaint Against','New Consultation Request Add Page',1
EXEC [dbo].pInsTranslation 'LegalAdvice_SubType',N'نوع الفتوى','Legal Advice Type','Detail Consultation Request',1
EXEC [dbo].pInsTranslation 'Author_Name_100',N'(100) اسم المؤلف','Author Name (100)','Literature',1


---------------- 15-5-2023 --------------------------------
EXEC [dbo].pInsTranslation 'Court_level',N'درجة التقاضي','Court Level','G2G Case file detail',1
EXEC [dbo].pInsTranslation 'Partial_And_Urgent_Cases',N'القضايا الجزئية المستعجلة','Partial And Urgent Cases','List Case File',1

----------------25-5-2023 --------------------------------
EXEC [dbo].pInsTranslation 'Are_you_sure_you_want_to_add_this_borrower',N'هل أنت متأكد من إضافة هذا المستعير','Are you sure you want to add this borrower','AddLmsLiteratureBorrowDetail page',1

------------------------------- Translation ( 02/06/2023)
EXEC [dbo].pInsTranslation 'Tasks',N'المهام العامة','Tasks','Tasks',1
EXEC [dbo].pInsTranslation 'Assignment',N'الأعضاء','Assignment','Case File View Page',1


-------------------------------- Translations ( 06/06/2023) ---------------------------------
EXEC [dbo].pInsTranslation 'Legal_Notifications',N'الاخطارات','Legal Notifications','General',1
EXEC [dbo].pInsTranslation 'Legislation_Advance_Search',N'البحث المتقدم','Legislation Advance Search','Inventory Item Detail',1
EXEC [dbo].pInsTranslation 'Barcode_No',N'رقم الباركود','Barcode Number','Legislation List',1
EXEC [dbo].pInsTranslation 'Borrow_Date',N'تاريخ الاستعارة','Borrow Date','Legislation List',1
EXEC [dbo].pInsTranslation 'Return_Date',N'تاريخ الارجاع','Return Date','Legislation List',1

----05-06-2023-------------------------
-------------Inventory Management----------------------
EXEC [dbo].pInsTranslation 'Item_Code',N'رمز الصنف','Item Code','Inventory Item Detail',1
EXEC [dbo].pInsTranslation 'Inventory_Management',N'ادارة المخازن','Inventory Management','Inventory Item Detail',1
EXEC [dbo].pInsTranslation 'Item_Name',N'اسم الصنف','Item Name','Inventory Item Detail',1
EXEC [dbo].pInsTranslation 'Quantity',N'الكمية','Quantity','Inventory Item Detail',1
EXEC [dbo].pInsTranslation 'Item_Category',N'أنواع الأصناف','Item Category','Inventory Item Detail',1
EXEC [dbo].pInsTranslation 'Department_Name',N'اسم القسم','Department Name','Inventory Item Detail',1
EXEC [dbo].pInsTranslation 'Created_By',N'انشأ من قبل','Created By','Inventory Item Detail',1
EXEC [dbo].pInsTranslation 'Description',N'الوصف','Description','Inventory Item Detail',1
EXEC [dbo].pInsTranslation 'Special_Instruction',N'التعليمات الخاصة','Special Instruction','Inventory Item Detail',1
EXEC [dbo].pInsTranslation 'Request_Id',N'رقم الطلب','Request Id','Inventory Item Detail',1
EXEC [dbo].pInsTranslation 'Request_Status',N'حالة الطلب','Request Status','Inventory Item Detail',1
EXEC [dbo].pInsTranslation 'Status_Name',N'اسم الحالة','Status Name','Inventory Item Detail',1
EXEC [dbo].pInsTranslation 'Request_Date',N'تاريخ الطلب','Request Date','Inventory Item Detail',1
EXEC [dbo].pInsTranslation 'store_Name',N'اسم المتجر','store Name','INV_Service_Store',1
EXEC [dbo].pInsTranslation 'Store_Location',N'موقع المتجر','Store Location','INV_Service_Store',1
EXEC [dbo].pInsTranslation 'Available_Quantity ',N'الكمية المتوفرة','Available Quantity ','INV_Service_Store',1
EXEC [dbo].pInsTranslation 'Request_Number',N'رقم الطلب','Request Number','Item request list',1
EXEC [dbo].pInsTranslation 'Pending_Quantity',N'كمية معلقة','Pending Quantity','Item request list',1
EXEC [dbo].pInsTranslation 'Item_Status',N'حالة الصنف','Item Status','Item request list',1
EXEC [dbo].pInsTranslation 'List_of_Requests',N'قائمة الطلب','List of Requests','Item request list',1
 EXEC [dbo].pInsTranslation 'Submitted',N'تم التقديم','Submitted','Item request list',1
EXEC [dbo].pInsTranslation 'Approved_by_HOS',N'اعتمد من قبل رئيس القطاع','Approved by HOS','Item request list',1
EXEC [dbo].pInsTranslation 'Rejected_by_Custodian',N'تم الرفض من قبل أمين العهدة','Rejected by Custodian','Item request list',1
EXEC [dbo].pInsTranslation 'In_Progress',N'قيد العمل','In Progress','Item request list',1
EXEC [dbo].pInsTranslation 'Closed',N'مغلق','Closed','Item request list',1
EXEC [dbo].pInsTranslation 'Approved',N'معتمد','Approved','Item request list',1
EXEC [dbo].pInsTranslation 'Partially_Approved',N'معتمد جزئيا','Partially Approved','Item request list',1
EXEC [dbo].pInsTranslation 'Rejected',N'مرفوض','Rejected','Item request list',1
EXEC [dbo].pInsTranslation 'Delivered',N'تم الاستلام','Delivered','Item request list',1
EXEC [dbo].pInsTranslation 'Item_Request_List',N'تم قائمة طلب الصنف','Item Request List','Item request list',1
EXEC [dbo].pInsTranslation 'Service_Request_view_Details',N'تفاصيل طلب خدمة العرض ','Service Request view Details','Item request list',1
EXEC [dbo].pInsTranslation 'Requestor',N'الطالب','Requestor','Item request list',1
EXEC [dbo].pInsTranslation 'Item_List',N'قائمة الأصناف','Item List','Item request list',1
EXEC [dbo].pInsTranslation 'Service_Request_Details',N'تفاصيل طلب الخدمة','Service Request Details','Item request list',1
EXEC [dbo].pInsTranslation 'Are_you_sure_you_want_to_approve?',N'هل أنت متأكد أنك تريد الموافقة؟','Are you sure you want to approve?','Approve button',1
EXEC [dbo].pInsTranslation 'Reason_of_Rejection',N'سبب الرفض','Reason of Rejection','Reject button',1
EXEC [dbo].pInsTranslation 'Store_View_Detail',N'سبب الرفض','Store View Detail','Store view detail button',1
EXEC [dbo].pInsTranslation 'place_order_item',N'ضع طلب الشراء','place order item','Place Order',1
EXEC [dbo].pInsTranslation 'order_detail_view',N'عرض تفاصيل الشراء','order detail view','order detail view',1
EXEC [dbo].pInsTranslation 'Order_Id',N'رقم الشراء','Order Id','order detail view',1
EXEC [dbo].pInsTranslation 'order_list',N'قائمة المشتريات','order list','order detail view',1
EXEC [dbo].pInsTranslation 'Order_View_Details',N' تفاصيل عرض المشتريات ','Order View Details','order detail view',1
EXEC [dbo].pInsTranslation 'Are_you_sure_from _delete_the_record?',N' هل أنت متأكد من حذف السجل؟','Are you sure from delete the record?','order list',1
  -----English value only ----
  EXEC [dbo].pInsTranslation 'List_of_Store',N'List of Store','List of Store','List of Store',1
EXEC [dbo].pInsTranslation 'Assigned_User',N'Assigned User','Assigned User','store view detail',1
EXEC [dbo].pInsTranslation 'Created_To',N'Created To','Created To','List of Store',1
EXEC [dbo].pInsTranslation 'Created_From',N'Created From','Created From','List of Store',1
EXEC [dbo].pInsTranslation 'Store_Type',N'Store Type','Store Type','List of Store',1
EXEC [dbo].pInsTranslation 'Order_To',N'Order To','Order To','List of Store',1
EXEC [dbo].pInsTranslation 'Order_From',N'Order_From','Order From','List of Store',1
EXEC [dbo].pInsTranslation 'Remove_Store',N'Remove Store','Remove Store','List of Store',1
EXEC [dbo].pInsTranslation 'Edit_Store',N'Edit Store','Edit Store','List of Store',1
EXEC [dbo].pInsTranslation 'Assign_User',N'Assign User','Assign User','List of Store',1
EXEC [dbo].pInsTranslation 'Store_Incharge',N'Store Incharge','Store Incharge','List of Store',1
EXEC [dbo].pInsTranslation 'list_of_store_view_detail',N'list of store view detail','list of store view detail','List of Store',1
EXEC [dbo].pInsTranslation 'View_Details_Of_Store',N'View Details Of Store','View Details Of Store','List of Store',1
EXEC [dbo].pInsTranslation 'View_Details',N'View Details','View Details','List of Store',1
EXEC [dbo].pInsTranslation 'Store_Name',N'Store Name','Store Name','List of Store',1
EXEC [dbo].pInsTranslation 'Order_StatusEn',N'Order Status','Order Status','List of Store',1
EXEC [dbo].pInsTranslation 'Order_List',N'Order List','Order List','List of Store',1
EXEC [dbo].pInsTranslation 'Order_Status',N'Order Status','Order Status','List of Store',1
EXEC [dbo].pInsTranslation 'Store_Description',N'Store Description','Store Description','List of Store',1
EXEC [dbo].pInsTranslation 'Details_Of_Requests',N'Details Of Requests','Details Of Requests','Item request list',1
EXEC [dbo].pInsTranslation 'Details_Of_Items',N'Details Of Items','Details Of Items','Item request list',1
EXEC [dbo].pInsTranslation 'Item_Request_Details',N'Item Request Details','Item Request Details','Item request list',1
------------------------DMS TRANSLATION START
EXEC [dbo].pInsTranslation 'Document_List',N'قائمة المستندات','Document List','DMS',1
EXEC [dbo].pInsTranslation 'Sure_Add_Favourite_Document',N'هل أنت متأكد من أنك تريد إضافة هذا المستند إلى المفضلة؟ ','Are you sure you want to add this document to favorite?','DMS',1
EXEC [dbo].pInsTranslation 'Document_Added_To_Favourite',N'تمت إضافة المستند بنجاح إلى المفضلة','Document successfully added to favorite','DMS',1
EXEC [dbo].pInsTranslation 'Sure_Remove_Favourite_Document',N'هل أنت متأكد من أنك تريد إزالة هذا المستند من المفضلة؟ ','Are you sure you want to remove this document from favorite?','DMS',1
EXEC [dbo].pInsTranslation 'Remove_Favourite_Document_Success',N'تم إزالة المستند بنجاح من المفضلة','Document successfully removed from favorite','DMS',1
EXEC [dbo].pInsTranslation 'Document_Detail',N'تفاصيل المستند','Document Detail','DMS',1
EXEC [dbo].pInsTranslation 'Share_Document',N'مشاركة المستند','Share Document','DMS',1
EXEC [dbo].pInsTranslation 'Share',N'مشاركة','Share','DMS',1
EXEC [dbo].pInsTranslation 'No_User_Of_Department',N'هذا القسم ليس له مستخدم','This department has no user','DMS',1
EXEC [dbo].pInsTranslation 'Sure_Share_Document',N'هل أنت متأكد من أنك تريد مشاركة هذا المستند؟ ','Are you sure you want to share this document?','DMS',1
EXEC [dbo].pInsTranslation 'Document_Shared_Successfully',N'تمت مشاركة المستند بنجاح','Document shared successfully','DMS',1

------------------------DMS TRANSLATION END

EXEC [dbo].pInsTranslation 'Source_Document',N'مصدر المستند','Source Document','Legislation View Detail',1
EXEC [dbo].pInsTranslation 'legislation_Success_Message',N'تم الغاء النشر بنجاح','Legislation has been Saved Successfully','Legal Legislation Add Page',1

EXEC [dbo].pInsTranslation 'AppealJudgements',N'أحكام الاستئناف','Appeal Judgements','DMS',1
EXEC [dbo].pInsTranslation 'SupremeJudgements',N'أحكام التمييز','Supreme Judgements','DMS',1
EXEC [dbo].pInsTranslation 'AdministrativeComplaints',N'التظلمات الادارية','Administrative Complaints','DMS',1
EXEC [dbo].pInsTranslation 'LegalAdvice',N'الفتاوى','Legal Advice','DMS',1
------------------------------
EXEC [dbo].pInsTranslation 'Sector_To',N'قطاع ل','Sector To','Case Request view',1
EXEC [dbo].pInsTranslation 'Sector_From',N'قطاع من','Sector From','Case Request view',1
EXEC [dbo].pInsTranslation 'Initial_Judgement',N'الحكم الأولي','Initial Judgement','Legal Notification',1
EXEC [dbo].pInsTranslation 'Final_Judgement',N'الحكم النهائي','Final Judgement','Legal Notification',1
EXEC [dbo].pInsTranslation 'Document_Management',N'إدارة المستندات','Document Management','Case file view',1
EXEC [dbo].pInsTranslation 'Court_level',N'مستوى المحكمة','Court level','Case file view',1
EXEC [dbo].pInsTranslation 'is_Favourite',N'هو المفضل','is Favourite','Document ',1
EXEC [dbo].pInsTranslation 'Assign_File',N'Assign File','Assign File','Assign file button',1

EXEC [dbo].pInsTranslation 'Sub_Module',N'الوحدة الفرعية','Sub Module','Meeting Add',1
EXEC [dbo].pInsTranslation 'Fatwa_Representative_Name',N'اسم ممثل إدارة الفتوى والتشريع','Fatwa Representative Name','Send Communication',1
EXEC [dbo].pInsTranslation 'View_Meeting',N'عرض الاجتماع','View Meeting','View Meeting page',1
EXEC [dbo].pInsTranslation 'Initial_Judgement',N'الحكم الأولي','Initial Judgement','Legal Notification',1
EXEC [dbo].pInsTranslation 'Final_Judgement',N'الحكم النهائي','Final Judgement','Legal Notification',1

EXEC [dbo].pInsTranslation 'Room_Number',N'رقم القاعة','Room Number','Create Registered cases',1
EXEC [dbo].pInsTranslation 'Approve',N'اعتماد','Approve','popup',1
EXEC [dbo].pInsTranslation 'Approve',N'اعتماد','Approve','popup',1

EXEC [dbo].pInsTranslation 'Govt_Entity',N'Govt Entity','Govt Entity','User Task list',1
EXEC [dbo].pInsTranslation 'Cannot_assign_to_this_laywer_Because_already_rejected',N'Cannot assign to this laywer Because already rejected','Cannot assign to this laywer Because already rejected','Case File View',1
EXEC [dbo].pInsTranslation 'Claim_Statement',N'Claim Statement','Claim Statement','Case File view',1

EXEC [dbo].pInsTranslation 'Additional_GE_Users',N'Additional GE Users','Additional GE Users','Request need more detail G2G',1
----------------------------------------

-------------------------------- Translation(25/07/2023)
EXEC [dbo].pInsTranslation 'Execution_Request_Created_For_Review',N'تم إنشاء طلب التنفيذ بنجاح ','Execution Request Created','CMS',1
EXEC [dbo].pInsTranslation 'Interpretation_Judgment',N'تفسير الحكم','Interpretation Of Judgement','General',1
EXEC [dbo].pInsTranslation 'Invalidity_Judgment',N'بطلان الحكم','Invalidity Of Judgement Execution','General',1 
EXEC [dbo].pInsTranslation 'Legal_Notification_Details',N'تفاصيل الإخطار القانوني ','Legal Notification Details','CMS',1
EXEC [dbo].pInsTranslation 'Legal_Notification_Response_Detail',N'تفاصيل استجابة الإخطار القانوني','Legal Notification Response Detail','CMS',1
EXEC [dbo].pInsTranslation 'Send_Response',N'أرسل جوابا','Send Response','General',1
EXEC [dbo].pInsTranslation 'Fill_Cannumber',N'Fill CAN Number','Fill CAN Number','CMS',1 
EXEC [dbo].pInsTranslation 'Compare_FileDraft',N'قارن مسودة الملف ','Compare File Draft','General',1
EXEC [dbo].pInsTranslation 'Case/Consultation_Management',N'إدارة القضية/الاستشارة','Case/Consultation Management','CMS',1
EXEC [dbo].pInsTranslation 'POO_Tasks',N'المكتب الفني الخاص','POO Tasks','CMS',1
EXEC [dbo].pInsTranslation 'Number_Allowed',N'الرقم المسموح به','Number Allowed','General',1
EXEC [dbo].pInsTranslation 'Sticker_And_Barcode',N'','Sticker And Barcode','Legal Literature Detail Page',1

EXEC [dbo].pInsTranslation 'Legislation_Issue_Date_Hijir',N'تاريخ اصدار التشريع (هجري)','Legislation Issue Date (Hijir)','Legislation List View Page',1
EXEC [dbo].pInsTranslation 'Add_MOM_Meeting_Success',N'تم إنشاء محضر اجتماع جديد #entity# بنجاح في النظام. يرجى زيارة الرابط المقدم لمزيد من التفاصيل.','New Minutes of Meeting #entity# is successfully create in the system. Please visit provided link for more details.','Case Request Detail',1

EXEC [dbo].pInsTranslation 'GE_Attendee_Accept_Meeting_Invite',N'GE Has Accepted Your Meeting #entity# Request. Please visit provided link for more details.','GE Has Accepted Your Meeting #entity# Request. Please visit provided link for more details.','Meeyting Notifications',1
EXEC [dbo].pInsTranslation 'Attendee_Accept_Meeting_Invite',N'Attendee Has Accepted Your Meeting #entity# Invite. Please visit provided link for more details.','Attendee Has Accepted Your Meeting #entity# Invite. Please visit provided link for more details.','Meeyting Notifications',1
EXEC [dbo].pInsTranslation 'Decision_Pending_Of_Attendee',N'New Meeting Invite Is Pending For Your  Decision #entity#. Please visit provided link for more details.','New Meeting Invite Is Pending For Your  Decision #entity#. Please visit provided link for more details.','Meeyting Notifications',1
EXEC [dbo].pInsTranslation 'Send_Meeting_To_HOS_Success',N'Decision is pending for creating a meeting #entity#. Please visit provided link for more details.','Decision is pending for creating a meeting  #entity#. Please visit provided link for more details.','Meeyting Notifications',1


EXEC [dbo].pInsTranslation 'Legislation_Issue_Date_Hijir',N'تاريخ اصدار التشريع (هجري)','Legislation Issue Date (Hijir)','Legislation List View Page',1


/*<History Author='Umer Zaman' Date='24-12-2023'> Meeting translation start </History>*/
EXEC [dbo].pInsTranslation 'Mom_Decision',N'محضر قرار الاجتماع','Minute of Meeting Decision','Minute of Meeting Decision page',1
EXEC [dbo].pInsTranslation 'Mom_MetaData_Information',N'قسم محضر الاجتماع','Minute of Meeting Section','Minute of Meeting Decision page', 1
EXEC [dbo].pInsTranslation 'Review_MOM',N'مراجعة محضر الاجتماع','Review Minutes of Meeting','Review Minutes of Meeting in Meeting Request',1
EXEC [dbo].pInsTranslation 'Request_For_Meeting',N'طلب للحصول على تفاصيل الاجتماع','Request For Meeting Detail','Request For Meeting Detail Page',1
EXEC [dbo].pInsTranslation 'Approve_Meeting_Request',N'الموافقة على طلب الاجتماع','Approve Meeting Request','Request For Meeting Decision Page',1
EXEC [dbo].pInsTranslation 'Reject_Meeting_Request',N'رفض طلب الاجتماع','Reject Meeting Request','Request For Meeting Decision Page',1
EXEC [dbo].pInsTranslation 'Save_and_Send_To_Attendees',N'حفظ وإرسالها إلى الحضور','Save And Send To Attendees','Request For Meeting Decision Page',1
EXEC [dbo].pInsTranslation 'Fatwa_Attendees',N'الفتوى الحاضرين','Fatwa Attendees','Meeting Details Page',1
EXEC [dbo].pInsTranslation 'Meeting_Approval_Success',N'تم حفظ طلب الموافقة على الاجتماع بنجاح','The meeting approval request was saved successfully','Meeting Details Page',1
EXEC [dbo].pInsTranslation 'Review_Meeting_Request',N'مراجعة طلب الاجتماعات','Review Meeting Request','Meeting Details Page',1
EXEC [dbo].pInsTranslation 'Add_Reason',N'أضف سببًا','Add Reason','Meeting Details Page',1
/*<History Author='Umer Zaman' Date='24-12-2023'> Meeting translation end </History>*/
EXEC [dbo].pInsTranslation 'Legislation_Issue_Date_Hijir',N'تاريخ اصدار التشريع (هجري)','Legislation Issue Date (Hijir)','Legislation List View Page',1
EXEC [dbo].pInsTranslation 'Legislation_Issue_Date_Hijir',N'تاريخ اصدار التشريع (هجري)','Legislation Issue Date (Hijir)','Legislation List View Page',1
EXEC [dbo].pInsTranslation 'Legislation_Issue_Date_Hijir',N'تاريخ اصدار التشريع (هجري)','Legislation Issue Date (Hijir)','Legislation List View Page',1

-------------------------------- Translation(31/10/2023)
--------------------------------Workflow


EXEC [dbo].pInsTranslation 'ConsultationManagement',N'نظام إدارة ملفات الاستشاري','Consultation Management','Workflow',1
EXEC [dbo].pInsTranslation 'CaseManagement',N'نظام إدارة ملفات القضايا','Case Management','Workflow',1

------------------------20/12/2023
EXEC [dbo].pInsTranslation 'View_Case_File_Details',N'عرض تفاصيل الحالة','View Case Details','Meeting',1
EXEC [dbo].pInsTranslation 'List_MOM_Documents',N'قائمة محضر وثائق الاجتماع','List Minutes of Meeting Documents','Meeting',1
EXEC [dbo].pInsTranslation 'Sure_Create_Meeting',N'هل أنت متأكد من إنشاء اجتماع؟','Are you sure to create meeting?','Meeting',1
EXEC [dbo].pInsTranslation 'View_Case_File_Details',N'عرض تفاصيل الحالة','View Case Details','Meeting',1
EXEC [dbo].pInsTranslation 'Meeting_Held_Decision',N'قرار الاجتماع','Meeting Decision','Meeting',1
EXEC [dbo].pInsTranslation 'Is_Held',N'يقام','Is Held','Meeting',1
EXEC [dbo].pInsTranslation 'Meeting_Held',N'انعقاد الاجتماع','Meeting Held','Meeting',1
EXEC [dbo].pInsTranslation 'MOM_Saved',N'محضر الاجتماع المحفوظة','Minutes of Meeting Saved','Meeting',1
EXEC [dbo].pInsTranslation 'Decicion_Save_Success',N'لقد تم اتخاذ القرار بنجاح.','Decision hase been Taken successfully.','Meeting',1
EXEC [dbo].pInsTranslation 'Sure_Submit_MOM',N'هل أنت متأكد من تقديم محضر الاجتماع؟','Are you sure to submit minutes of meeting?','Meeting',1
EXEC [dbo].pInsTranslation 'MOM_Fatwa_Attendee',N'محضر اجتماع الفتوى','Minutes of Meeting Fatwa Attendee','Meeting',1
EXEC [dbo].pInsTranslation 'MOM_GE_Attendee',N'محضر اجتماع GE الحضور','Minutes of Meeting GE Attendee','Meeting',1
EXEC [dbo].pInsTranslation 'MOM_Submit',N'تم إرسال محضر الاجتماع بنجاح.','Minutes of meeting submit successfully.','Meeting',1
EXEC [dbo].pInsTranslation 'Meeting_Held',N'يتم عقد الاجتماع بنجاح.','Meeting is held successfully.','Meeting',1
EXEC [dbo].pInsTranslation 'Meeting_Type',N'نوع الاجتماع','Meeting Type','Meeting',1
EXEC [dbo].pInsTranslation 'Reject_and_send_to_Ge',N'رفض وإرسالها إلى شركة جنرال إلكتريك','Reject and Send to GE','Meeting',1
EXEC [dbo].pInsTranslation 'Accept_and_send_to_Ge',N'قبول وإرسال إلى شركة جنرال إلكتريك','Accept and Send to GE','Meeting',1

/*<History Author='Umer Zaman' Date='22-12-2023'> Meeting translation start </History>*/
EXEC [dbo].pInsTranslation 'Send_To_HOS',N'إرسال إلى رئيس القطاع','Send To HOS','Save Meeting Page',1
EXEC [dbo].pInsTranslation 'Sure_Send_To_HOS',N'هل أنت متأكد أنك تريد إرسال الاجتماع إلى رئيس القطاع؟','Are you sure you want to send the meeting to the sector head? ','Save Meeting Page',1
/*<History Author='Umer Zaman' Date='22-12-2023'> Meeting translation start </History>*/

/*<History Author='Ammaar Naveed' Date='02-01-2024'> Forget Password Translations </History>*/
EXEC [dbo].pInsTranslation 'forget_password',N'نسيان كلمة المرور','Forgot Password?','ForgotPasswordDialog',1
EXEC [dbo].pInsTranslation 'forget_password_dialog_text',N'إذا كنت قد نسيت كلمة المرور الخاصة بك ، فيرجى التواصل مع مسؤول النظام','If you have forgotten your password please contact your HR or IT administrator.','ForgotPasswordDialog',1
UPDATE tTranslation SET Value_Ar='2024 Ⓒ مجلس الوزراء الفتوى والتشريع' WHERE TranslationKey='Fatwa_Footer'
UPDATE tTranslation SET Value_En='Council of Ministers Legal Advice and Legislation ? 2024' WHERE TranslationKey='Fatwa_Footer'
EXEC [dbo].pInsTranslation 'FATWA_ADMINISTRATION_PORTAL',N'FATWA Administration Portal','FATWA Administration Portal','MainLayoutAdmin',1

/*<History Author='Umer Zaman' Date='22-12-2023'> Meeting translation start </History>*/

-----02/01/2024
EXEC [dbo].pInsTranslation 'Add_Notes',N'أضف ملاحظات','Add Notes','Meeting',1
EXEC [dbo].pInsTranslation 'Meeting_Held',N'انعقاد الاجتماع','Meeting Held','Meeting',1
EXEC [dbo].pInsTranslation 'Submit_MOM_to_Ge',N'أرسل MOM إلى GE','Submit MOM To GE','Meeting',1
EXEC [dbo].pInsTranslation 'MOM_Draft',N'مسودة محضر الاجتماع','Minutes of Meeting Draft','Meeting',1


EXEC [dbo].pInsTranslation 'Suspended',N'موقوف','Suspended','Workflow List',1
EXEC [dbo].pInsTranslation 'InActive',N'غير نشط','InActive','Workflow List',1
EXEC [dbo].pInsTranslation 'Deleted',N'تم الحذف','Deleted','Workflow List',1
EXEC [dbo].pInsTranslation 'Workflows',N'آلية العمل ','Workflows','Workflow',1
EXEC [dbo].pInsTranslation 'Create_Workflow',N'إنشاء آلية العمل','Create Workflow','Workflows',1
EXEC [dbo].pInsTranslation 'Type_of_Draft',N'نوع المسودة','Type of Draft','Create Workflow',1
EXEC [dbo].pInsTranslation 'Trigger',N'Trigger','الحدث','Workflow List',1
EXEC [dbo].pInsTranslation 'CMS_Case_Management',N'إدارة ملفات القضايا','CMS Case Management','Workflow List',1
EXEC [dbo].pInsTranslation 'Workflow_Details',N'تفاصيل آلية العمل','Workflow Details','Workflow List',1
EXEC [dbo].pInsTranslation 'Workflow_Description',N'وصف آلية العمل','Workflow Description','Workflow List',1
EXEC [dbo].pInsTranslation 'Workflow_Trigger',N'حدث آلية العمل','Workflow Trigger','Create Workflow',1
EXEC [dbo].pInsTranslation 'Request_To',N'طلب إلى','Request To','Workflow List',1
EXEC [dbo].pInsTranslation 'Workflow_Module',N'وحدة آلية العمل','Workflow Module','Create Workflow',1
EXEC [dbo].pInsTranslation 'LegalLibrarySystem',N'نظام المكتبة القانونية','Legal Library System','Create Workflow',1
EXEC [dbo].pInsTranslation 'Advance_Detail',N'المزيد من التفاصيل ','Advance Detail','Create Workflow',1
EXEC [dbo].pInsTranslation 'Workflow_Activities',N'أنشطة آلية العمل','Workflow Activities','Create Workflow',1
EXEC [dbo].pInsTranslation 'Workflow_Conditions',N'شروط آلية العمل','Workflow Conditions','Create Workflow',1
EXEC [dbo].pInsTranslation 'Workflow_Options',N'خيارات آلية العمل','Workflow Options','Create Workflow',1
EXEC [dbo].pInsTranslation 'Trigger_Conditions',N'شروط الحدث','Trigger Conditions','Create Workflow',1
EXEC [dbo].pInsTranslation 'Conditions',N'شروط','Conditions','Create Workflow',1
EXEC [dbo].pInsTranslation 'Choose_Activity',N'اختر النشاط','Choose Activity','Create Workflow',1
EXEC [dbo].pInsTranslation 'Activity',N'نشاط','Activity','Create Workflow',1
EXEC [dbo].pInsTranslation 'FlowControls',N'ضوابط التدفق','Flow Controls','Create Workflow',1
EXEC [dbo].pInsTranslation 'Choose_Execution_Branch',N'اختر فرع التنفيذ','Choose Execution Branch','Create Workflow',1
EXEC [dbo].pInsTranslation 'ConditionalBranch',N'فرع مع شرط','Conditional Branch','Create Workflow',1
EXEC [dbo].pInsTranslation 'OptionBranch',N'فرع مع شرط','OptionBranch','Create Workflow',1
EXEC [dbo].pInsTranslation 'Conditional_Branch',N'فرع مع شرط','Conditional Branch','Create Workflow',1
EXEC [dbo].pInsTranslation 'Add_Condition',N'أضف الحالة','Add Condition','Create Workflow',1
EXEC [dbo].pInsTranslation 'Flow_Control',N'التحكم في التدفق','Flow Control','Create Workflow',1
EXEC [dbo].pInsTranslation 'JumptoStep',N'القفز إلى الخطوة','Jump to Step','Create Workflow',1
EXEC [dbo].pInsTranslation 'EndofFlow',N'نهاية الآلية','End of Flow','Create Workflow',1
EXEC [dbo].pInsTranslation 'Add_Options',N'أضف الخيارات','Add Options','Create Workflow',1
EXEC [dbo].pInsTranslation 'Delete_Options',N'حذف الخيارات','Delete Options','Create Workflow',1
EXEC [dbo].pInsTranslation 'View_Options',N'عرض الخيارات','View Options','Create Workflow',1
EXEC [dbo].pInsTranslation 'Condition_Option',N'خيار مع شرط','Condition Option','Create Workflow',1
EXEC [dbo].pInsTranslation 'Trigger_Options',N'خيارات الحدث','Trigger Options','Workflow',1
EXEC [dbo].pInsTranslation 'Add_Transfer_Option',N'إضافة خيار الإحالة','Add Transfer Option','Workflow',1
EXEC [dbo].pInsTranslation 'Transfer_Options',N'خيارات الإحالة','Transfer Options','Workflow',1
EXEC [dbo].pInsTranslation 'Transfer_To_PO_But_Send_TO_FP_For_Decision',N'إحالة إلى المكتب الفني الخاص وإرسال إلى رئيس الفتوى لاتخاذ القرار','Transfer To PO But Send To FP For Decision','Create Workflow',1
EXEC [dbo].pInsTranslation 'Option_Branch',N'خيار فرعي','Option Branch','Create Workflow',1
EXEC [dbo].pInsTranslation 'Please_provide_workflow_name',N'يرجى كتابة اسم لآلية العمل','Please provide workflow name','Create Workflow',1
EXEC [dbo].pInsTranslation 'No_Activites_For_Workflow',N'لم يتم العثور على أنشطة لآلية العمل. حاول مرة اخرى.','No Activities found for the workflow. Please try again.','Create Workflow',1
EXEC [dbo].pInsTranslation 'No_Endflow_For_Workflow',N'لم يتم العثور على نهاية آلية العمل. حاول مرة اخرى.','No EndofFlow found for the workflow. Please try again.','Create Workflow',1
EXEC [dbo].pInsTranslation 'Fill_Parameter_Activity',N'يرجى تعبئة متطلبات النشاط','Please fill required activity parameters.','Create Workflow',1
EXEC [dbo].pInsTranslation 'Are_you_sure_you_want_to_submit_workflow?',N'هل أنت متأكد من أنك تريد إرسال آلية العمل؟','Are you sure you want to submit workflow?','Workflow',1
EXEC [dbo].pInsTranslation 'Submit_Workflow',N'إرسال آلية العمل','Submit Workflow','Workflow',1
EXEC [dbo].pInsTranslation 'Workflow_Added_Successfully',N'تم تقديم آلية العمل بنجاح','Workflow has been submitted successfully','Workflow',1
EXEC [dbo].pInsTranslation 'Workflow_Draft_Added_Successfully',N'تمت إضافة مسودة آلية العمل بنجاح','Workflow draft has been added successfully','Workflow',1
EXEC [dbo].pInsTranslation 'Sure_Save_Workflow_As_Draft',N'هل أنت متأكد من أنك تريد حفظ آلية العمل كمسودة؟','Are you sure you want to save this workflow as a draft','Workflow',1
EXEC [dbo].pInsTranslation 'CaseManagement',N'نظام إدارة ملفات القضايا','Case Management','Create Workflow',1
EXEC [dbo].pInsTranslation 'ConsultationManagement',N'نظام إدارة ملفات الاستشاري','Consultation Management','Create Workflow',1
EXEC [dbo].pInsTranslation 'Total_Instances_Running',N'مجموع النماذج قيد العمل','Total Instances Running','Workflow',1
EXEC [dbo].pInsTranslation 'Total_Instances_Running_On',N'مجموع النماذج قيد العمل على','Total Instances Running On','Workflow',1
EXEC [dbo].pInsTranslation 'Workflow_Instances',N'نماذج آليات العمل','Workflow Instances','Workflow Instance List',1
EXEC [dbo].pInsTranslation 'Workflows_List',N'قائمة آليات العمل','Workflows List','Workflows List',1
EXEC [dbo].pInsTranslation 'Expired_Instances',N'غير فعال','Expired','Workflow Instance List',1
EXEC [dbo].pInsTranslation 'Cancelled_Instances',N'تم الإلغاء','Cancelled','Workflow Instance List',1
EXEC [dbo].pInsTranslation 'Failed_Instances',N'فشلت العملية','Failed','Workflow Instance List',1
EXEC [dbo].pInsTranslation 'Workflow_Management',N'إدارة  آليات العمل','Workflow Management','Create Workflow',1
EXEC [dbo].pInsTranslation 'Instance_Title',N'عنوان النموذج','Instance Title','Workflow Instance List',1

------- Ismail Translations
EXEC [dbo].pInsTranslation 'Under_Filing',N'تحت الرفع','Under Filing','Under Filing',1
EXEC [dbo].pInsTranslation 'Moj_Registration_Requests',N'طلبات تسجيل وزارة العدل','Moj Registration Requests','Moj Registration Requests',1
EXEC [dbo].pInsTranslation 'Correspondences',N'المراسلات','Correspondences','Correspondences',1
EXEC [dbo].pInsTranslation 'Moj_Execution_Requests',N'طلبات التنفيذ الخاصة بوزارة العدل','Moj Execution Requests','Moj Execution Requests',1
EXEC [dbo].pInsTranslation 'Transaction_Date',N'تاريخ العملية','Transaction Date','Transaction_Date',1
EXEC [dbo].pInsTranslation 'Activity',N'نشاط','Activity','Activity',1
EXEC [dbo].pInsTranslation 'Sent_By',N'مرسل من قبل','Sent By','Sent By',1
EXEC [dbo].pInsTranslation 'Sender_Name',N'اسم المرسل','Sender Name','Sender Name',1
EXEC [dbo].pInsTranslation 'Transaction_Type',N'نوع العملية','Transaction Type','Transaction Type',1
EXEC [dbo].pInsTranslation 'Reviewer',N'أرسل إلى','Reviewer','Reviewer',1

EXEC [dbo].pInsTranslation 'Chamber_And_Court_Details',N'تفاصيل الدائرة والمحكمة','Chamber And Court Details','Chamber And Court Details',1
EXEC [dbo].pInsTranslation 'Case_Tasks',N'مهام القضية','Case Tasks','Case Tasks',1
EXEC [dbo].pInsTranslation 'View_File_Details',N'عرض تفاصيل الملف','View File Details','View File Details',1
EXEC [dbo].pInsTranslation 'Remarks_For_Rejection',N'ملاحظات','Remarks For Rejection','Remarks For Rejection',1
EXEC [dbo].pInsTranslation 'Pending',N'قيد الانتظار','Pending','Pending',1
EXEC [dbo].pInsTranslation 'Completed',N'مكتمل','Completed','Completed',1
EXEC [dbo].pInsTranslation 'Action_Required',N'الإجراء مطلوب','Action Required','Action Required',1
EXEC [dbo].pInsTranslation 'Govt_Entity',N'الجهة الحكومية','Govt Entity','Govt Entity',1
EXEC [dbo].pInsTranslation 'Document_Type',N'نوع المستند','Document Type','Document Type',1
EXEC [dbo].pInsTranslation 'Action_Item',N'الإجراءات','Action Item','Action Item',1
EXEC [dbo].pInsTranslation 'Task_Number',N'رقم المهمة','Task Number','Task Number',1
EXEC [dbo].pInsTranslation 'Attachment',N'المرفقات','Attachment','Attachment',1
EXEC [dbo].pInsTranslation 'Attachment_Details',N'تفاصيل المرفقات','Attachment Details','Attachment Details',1
EXEC [dbo].pInsTranslation 'Sub_Type',N'نوع المهمة','Sub Type','Sub Type',1
EXEC [dbo].pInsTranslation 'Url',N'الرابط','Url','Url',1
EXEC [dbo].pInsTranslation 'Todo_List',N'قائمة المهام المراد إنجازها','To Do List','To Do List',1
EXEC [dbo].pInsTranslation 'FileNoOrRequestNo',N'رقم الملف / رقم الطلب','.File No. / Request No.','.File No. / Request No.',1
EXEC [dbo].pInsTranslation 'Record_Date',N'تاريخ السجل','Record Date','Record Date',1
EXEC [dbo].pInsTranslation 'Status_History',N'تغيرات حالة المستخدم','User Status History','User Status History',1
EXEC [dbo].pInsTranslation 'Case_Request_Detail_View',N'تفاصيل طلب رفع القضية','Case Registered Requests Detail','Case Registered Requests Detail',1
EXEC [dbo].pInsTranslation 'Claim_Amount',N'مبلغ القضية','Claim Amount','Claim Amount',1
EXEC [dbo].pInsTranslation 'History_Of_Case_Request',N'التغيرات على طلب رفع قضية','History Of Case Request','History Of Case Request',1
EXEC [dbo].pInsTranslation 'Case_Requirements',N'يجب ادخال ملخص الطلبات','Case Requirements','Case Requirements',1
EXEC [dbo].pInsTranslation 'CivilId/CRN',N'رقم البطاقة المدنية/رقم السجل التجاري','Civil ID/CRN','Civil ID/CRN',1
EXEC [dbo].pInsTranslation 'Case_Number',N'رقم القضية','Case No.','Case No.',1
EXEC [dbo].pInsTranslation 'CAN_Number',N'الرقم الآلي للقضية','CAN No.','CAN No.',1
EXEC [dbo].pInsTranslation 'Task_Dashboard',N'لوحة المهام','Task Dashboard','Task Dashboard',1
EXEC [dbo].pInsTranslation 'Linked_Requests',N'الطلبات المرتبطة','Linked Requests','Linked Requests',1
EXEC [dbo].pInsTranslation 'Workflow_Management',N'إدارة  آليات العمل','Workflow Management','Create Workflow',1
EXEC [dbo].pInsTranslation 'Instance_Title',N'عنوان النموذج','Instance Title','Workflow Instance List',1

/*<History Author='ijaz Ahmad' Date='09-01-2024'> Kuwait Alyawm Publication Document list translation  </History>*/
EXEC [dbo].pInsTranslation 'Document_List_Kuwait_Alyawm',N'قائمة مستندات كويت اليوم','Document List Kuwait Al-yawm',' Kuwait Alyawm list',1
EXEC [dbo].pInsTranslation 'Edition_Number',N'رقم الإصدار','Edition Number',' Kuwait Alyawm list',1
EXEC [dbo].pInsTranslation 'Edition_Type',N'نوع الطبعة','Edition Type',' Kuwait Alyawm list',1
EXEC [dbo].pInsTranslation 'Document_Title',N'عنوان المستند','Document Title',' Kuwait Alyawm list',1
EXEC [dbo].pInsTranslation 'Publication_Date',N'تاريخ النشر','Publication Date',' Kuwait Alyawm list',1
EXEC [dbo].pInsTranslation 'Date_From',N'من تاربخ','Date From','General',1
EXEC [dbo].pInsTranslation 'Date_To',N'إلى تاربخ','Date To','General',1
EXEC [dbo].pInsTranslation 'KuwaitAlyawm',N'كويت اليوم','Kuwait Al-yawm','General',1
EXEC [dbo].pInsTranslation 'Must_Select_Atleast_One_Source_Either_from_Kuwait',N' اختر مصدرًا واحدًا على الأقل من "الكويت اليوم" أو من مصدر خارجي',' Select Atleast One Source Document Either from Kuwait Alyawm or External Source','General',1

---------------Govt Entity Department ------------------
 
EXEC [dbo].pInsTranslation 'Government_Entity_Department_History',N'التغيرات على أقسام الجهات الحكومية','Government Entity Department History ','GE Department Lookup',1
EXEC [dbo].pInsTranslation 'Government_Entities_Department',N'أقسام الجهات الحكومية','Government Entities Department ',' GE Department Lookup',1
EXEC [dbo].pInsTranslation 'Are_you_Sure_You_Want_to_Delete_Government_Entity_Department',N'هل أنت متأكد أنك تريد حذف قسم الجهة الحكومية؟','Are you Sure You Want to Delete Government Entity Department',' GE Department Lookup',1
EXEC [dbo].pInsTranslation 'Edit_Government_Entity_Department_Detail',N'تعديل تفاصيل قسم الجهة الحكومية','Edit Government Entity Department Detail',' GE Department Lookup',1
EXEC [dbo].pInsTranslation 'Add_Government_Entity_Department_Detail',N'إضافة تفاصيل قسم الجهة الحكومية','Add Government Entity Department Detail',' GE Department Lookup',1
EXEC [dbo].pInsTranslation 'Government_Entity_Department_Added_Successfully',N'تم إضافة قسم الجهة الحكومية بنجاح','Government Entity Department Added Successfully',' GE Department Lookup',1
EXEC [dbo].pInsTranslation 'Could_not_create_a_new_Government_Entity_Department',N'لا يمكن إنشاء قسم جديد للجهة الحكومية','Could Not Create a New Government Entity Department',' GE Department Lookup',1
EXEC [dbo].pInsTranslation 'Government_Entity_Department_Updated_Successfully',N'تم تعديل قسم الجهة الحكومية بنجاح','Government Entity Department Updated Successfully',' GE Department Lookup',1
EXEC [dbo].pInsTranslation 'Government_Entity_Department_could_not_be_updated',N'لا يمكن تعديل قسم الجهة الحكومية','Government Entity Department Could Not be Updated',' GE Department Lookup',1

 
EXEC [dbo].pInsTranslation 'Instance_Title',N'عنوان النموذج','Instance Title','Workflow Instance List',1


--------------------------
EXEC [dbo].pInsTranslation 'Delete_Attendee_From_Meeting',N'لقد تم حذفك من هذا الاجتماع #entity#. يرجى زيارة الرابط المقدم لمزيد من التفاصيل.','You have been deleted from this meeting #entity#. Please visit provided link for more details.','Meeting Creation Page',1
--------------------------

------Ammaar Naveed-----11/01/2024---------
EXEC [dbo].pInsTranslation 'Add_New_Employee',N'إضافة موظف جديد','Add New Employee','AddEmployee',1
EXEC [dbo].pInsTranslation 'Fingerprint_Id',N'رقم البصمة','Fingerprint Id','AddEmployee',1
EXEC [dbo].pInsTranslation 'Login_Id',N'رقم تسجيل الدخول','Login Id','AddEmployee',1
EXEC [dbo].pInsTranslation 'Add_External_Employee',N'إضافة موظف من خارج الإدارة','Add External Employee','AddEmployee',1
EXEC [dbo].pInsTranslation 'Add_Internal_Employee',N'إضافة موظف من داخل الإدارة','Add Internal Employee','AddEmployee',1
EXEC [dbo].pInsTranslation 'Confirm_Add_Employee',N'هل أنت متأكد أنك تريد إضافة هذا الموظف؟','Are you sure you want to add this employee?','AddEmployee',1
UPDATE tTranslation SET Value_En='OK' WHERE TranslationKey='Ok'
EXEC [dbo].pInsTranslation 'Employee_Management',N'إدارة شؤون الموظفين','Employee Management','MainScreen',1
EXEC [dbo].pInsTranslation 'Add_Employee',N'إضافة الموظف','Add Employee','Employee List',1
EXEC [dbo].pInsTranslation 'Bulk_Import',N'الاستيراد بالجملة','Bulk Import','Employee List',1
UPDATE tTranslation SET Value_En='Advance Search' WHERE TranslationKey='Advanced_Search'
EXEC [dbo].pInsTranslation 'Internal_Employee',N'الموظف الداخلي','Internal Employee','Employee List',1
EXEC [dbo].pInsTranslation 'External_Employee',N'الموظف الخارجي','External Employee','Employee List',1
EXEC [dbo].pInsTranslation 'Employee_List',N'قائمة الموظف','Employee List','Add Employee',1
UPDATE tTranslation SET Value_En='Second Name (English)' WHERE TranslationKey='SecondName_En'
UPDATE tTranslation SET Value_En='Second Name (Arabic)' WHERE TranslationKey='SecondName_Ar'

------Ammaar Naveed-----12/01/2024---------
EXEC [dbo].pInsTranslation 'Personal_Information',N'المعلومات الشخصية','Personal Information','Add Employee',1
EXEC [dbo].pInsTranslation 'Employment_Information',N'معلومات التوظيف','Employment Information','Add Employee',1
EXEC [dbo].pInsTranslation 'Add_Address',N'اضف عنوان','Add Address','Add Employee',1

------Ammaar Naveed-----15/01/2024---------
EXEC [dbo].pInsTranslation 'Employee_Added_Success_External',N'تم إنشاء الموظف بنجاح، كما تم إنشاء كلمة المرورالافتراضية والتي سيتم مشاركتها مع رقم المستخدم لتسجيل الدخول','The employee has been successfully created, the default password has also been generated which should be communicated along with the login id for login','Add Employee Success',1
EXEC [dbo].pInsTranslation 'Employee_Added_Success_AD',N'تم إنشاء الموظف بنجاح في Active Directory، كما تم إنشاء كلمة المرور الافتراضية والتي سيتم مشاركتها مع رقم المستخدم لتسجيل الدخول','The employee has been successfully created in the active directory, the default password has also been generated which should be communicated along with the login id for login.','Add Employee Success',1
EXEC [dbo].pInsTranslation 'Employee_Added_Successfully',N'تم إضافة الموظف بنجاح','Employee Added Successfully','Add Employee Success',1
EXEC [dbo].pInsTranslation 'Go_To_Users_List',N'اذهب إلى قائمة المستخدمين','Go to Users List','Add Employee Success',1
EXEC [dbo].pInsTranslation 'Confirm_Update_Employee',N'هل أنت متأكد أنك تريد تعديل تفاصيل الموظف؟','Are you sure you want to update employee details?','Update/Edit Employee',1
EXEC [dbo].pInsTranslation 'Employee_Updated_Successfully',N'تم تعديل تفاصيل الموظف بنجاح','Employee details have been updated successfully','Update/Edit Employee',1
EXEC [dbo].pInsTranslation 'Add_Additional_Information',N'إضافة بيانات إضافية','Add Additional Information','Add Employee Success',1
EXEC [dbo].pInsTranslation 'Employee_Type',N'نوع الموظف','Employee Type','Add Employee',1
EXEC [dbo].pInsTranslation 'Add_Employee_Type',N'إضافة نوع الموظف','Add Employee Type','Add Employee',1

------Ammaar Naveed-----16/01/2024---------
EXEC [dbo].pInsTranslation 'Employee_Id',N'رقم ID الموظف','Employee Id','Add Employee Dialog',1

----------------- ismail 12/01/2024 
EXEC [dbo].pInsTranslation 'Attended',N'تم الحضور','Attended','meeting detail',1
EXEC [dbo].pInsTranslation 'GE_Attendee',N'الحاضرين من الجهة الحكومية','GE Attendee','meeting detail',1
Update tTranslation Set Value_Ar = N'2023 Ⓒ مجلس الوزراء - الفتوى والتشريع' where TranslationKey = 'Fatwa_Footer'
Update tTranslation set Value_Ar = N'العضو الذي تم تعيينه إلى قائمة المحكمة' where TranslationKey = 'Assigned_lawyer_list'
Update tTranslation Set Value_Ar =N'طلب حافظة مستندات' where TranslationKey = 'Moj_Document_Portfolio_Requests' 
Update tTranslation Set Value_Ar =N'طلبات التنفيذ الخاصة بوزارة العدل' where TranslationKey = 'Moj_Execution_Requests' 
Update tTranslation Set Value_Ar =N'تفاصيل تسجيل القضية في المحكمة' where TranslationKey = 'Moj_Registration_Detail' 
Update tTranslation Set Value_Ar =N'إضافة تفاصيل القضية' where TranslationKey = 'Add_Case_Detail' 
Update tTranslation Set Value_Ar =N'نوع المحكمة' where TranslationKey = 'Court_Type' 
Update tTranslation Set Value_Ar =N'اختر المحكمة' where TranslationKey = 'Select_Court' 
Update tTranslation Set Value_Ar =N'اختر الدائرة' where TranslationKey = 'Select_Chamber' 
Update tTranslation Set Value_Ar =N'تفاصيل الجلسة' where TranslationKey = 'Hearing_Details' 
Update tTranslation Set Value_Ar =N'تاريخ الجلسة' where TranslationKey = 'Hearing_Date' 
Update tTranslation Set Value_Ar =N'وقت الجلسة' where TranslationKey = 'Hearing_Time' 
Update tTranslation Set Value_Ar =N'هذه الخانة مطلوبه.' where TranslationKey = 'Required_Field' 
-------Admin Portal 
EXEC [dbo].pInsTranslation 'Add_Case_File_Status',N'إضافة حالة ملف القضية','Case File Status Add','General',1
EXEC [dbo].pInsTranslation 'Case_File_Status_(G2G_And_Fatwa)',N'حالة ملف القضية (الفتوى والبوابة البينية)','Case File Status (G2G And Fatwa)','General',1
EXEC [dbo].pInsTranslation 'Case_File_Status_Updated_Successfully',N'تم تحديث حالة ملف القضية بنجاح','Case File Status Updated Successfully',1
EXEC [dbo].pInsTranslation 'Case_File_Status_could_not_be_updated',N'لا يمكن تحديث حالة ملف القضية','Case File Status could not be updated',1
EXEC [dbo].pInsTranslation 'Add_Case_Request_Status',N'إضافة حالة طلب القضية','Add Case Request Status',1
EXEC [dbo].pInsTranslation 'Case_Request_Status_Upadated_Successfully',N'تم تحديث حالة طلب القضية بنجاح','Case Request Status Updated Successfully',1
EXEC [dbo].pInsTranslation 'Case_Request_Status_could_not_be_updated',N'لا يمكن تحديث حالة طلب القضية ','Case Request Status could not be updated','Case Request Status',1
 EXEC [dbo].pInsTranslation 'Case_File_Status',N'حالة ملف القضية ','Case File Status','General',1
EXEC [dbo].pInsTranslation 'Case_File_Status_Upadated_Successfully',N'تم تحديث حالة طلب القضية بنجاح','Case File Status Updated Successfully',1
EXEC [dbo].pInsTranslation 'Case_File_Status_could_not_be_updated',N'لا يمكن تحديث حالة طلب القضية ','Case File Status could not be updated',1
 EXEC [dbo].pInsTranslation 'Edit_Government_Entity',N'تعديل الجهة الحكومية','Edit Government Entity','Government Entity',1
EXEC [dbo].pInsTranslation 'Add_Government_Entity',N'إضافة الجهة الحكومية','Add Government Entity','Government Entity',1
EXEC [dbo].pInsTranslation 'Are_you_Sure_you_want_to_update_Status',N'هل أنت متأكد أنك تريد تحديث الحالة؟','Are you Sure you want to update Status','Government Entity',1
EXEC [dbo].pInsTranslation 'Updated_Successfully',N'تم التحديث بنجاح','Updated Successfully','Government Entity',1
EXEC [dbo].pInsTranslation 'Edit_Court_Detail',N'تعديل تفاصيل المحكمة','Edit Court Detail','Court Type',1
EXEC [dbo].pInsTranslation 'Add_Court_Detail',N'إضافة تفاصيل المحكمة','Add Court Detail','Court Type',1
EXEC [dbo].pInsTranslation 'Court_type',N'نوع المحكمة','Court type','Court Type',1
EXEC [dbo].pInsTranslation 'Court_Code',N'رقم المحكمة','Court Code','Court Type',1
EXEC [dbo].pInsTranslation 'Court_Code_is_required',N'رقم المحكمة مطلوب','Court Code is required','Court Type',1
EXEC [dbo].pInsTranslation 'Add_Chamber_Detail',N'إضافة تفاصيل الدائرة','Add Chamber Detail','Chamber Detail',1
EXEC [dbo].pInsTranslation 'Edit_Chamber_Detail',N'تعديل تفاصيل الدائرة','Edit Chamber Detail','Chamber Detail',1
EXEC [dbo].pInsTranslation 'Are_you_Sure_You_Want_to_Delete_Chamber',N'هل أنت متأكد من أنك تريد حذف الدائرة؟','Are you Sure You Want to Delete Chamber','Chamber Detail',1
EXEC [dbo].pInsTranslation 'Add_Department_Detail',N'إضافة تفاصيل القسم','Add Department Detail','Department Detail',1
EXEC [dbo].pInsTranslation 'Edit_Department_Detail',N'تعديل تفاصيل القسم','Edit Department Detail','Department Detail',1
EXEC [dbo].pInsTranslation 'Are_you_Sure_You_Want_to_Delete_Department',N'هل أنت متأكد أنك تريد حذف القسم؟','Are you Sure You Want to Delete Department','Department Detail',1
EXEC [dbo].pInsTranslation 'Edit_Communication_Detail',N'تعديل تفاصيل التواصل','Edit Communication Detail','Communication Detail',1
EXEC [dbo].pInsTranslation 'Edit_Case_File_Status_Detail',N'تعديل تفاصيل حالة ملف القضية','Edit Case File Status Detail','Case Detail',1
EXEC [dbo].pInsTranslation 'Edit_Case_Request_Status_Detail',N'تعديل تفاصيل حالة طلب القضية','Edit Case Request Status Detail',1
EXEC [dbo].pInsTranslation 'Edit_Case_Status_Detail',N'تعديل تفاصيل حالة القضية','Edit Case Status Detail','Case Detail',1
EXEC [dbo].pInsTranslation 'Edit_Task_Detail',N'تعديل تفاصيل المهمة','Edit Task Detail','Task Detail',1
EXEC [dbo].pInsTranslation 'Add_Document_Detail',N'إضافة تفاصيل المستند','Add Document Detail','Document Detail',1
EXEC [dbo].pInsTranslation 'Edit_Document_Detail',N'تعديل تفاصيل المستند','Edit Document Detail','Document Detail',1
EXEC [dbo].pInsTranslation 'Type_En',N'اكتب بالانجليزي','Type En','Document Detail',1
EXEC [dbo].pInsTranslation 'Type_Ar',N'اكتب بالعربي','Type Ar','Document Detail',1
EXEC [dbo].pInsTranslation 'Module_NameEn',N'اسم الوحدة انجليزي','Module NameEn','Document Detail',1
EXEC [dbo].pInsTranslation 'Subtype_En',N'النوع الفرعي انجليزي','Subtype En','Document Detail',1
EXEC [dbo].pInsTranslation 'Task_Type_Updated_Successfully',N'تم تحديث نوع المهمة بنجاح','Task Type Updated Successfully','Task Detail',1
EXEC [dbo].pInsTranslation 'Task_Type_could_not_be_updated',N'لا يمكن تحديث نوع المهمة','Task Type could not be updated','Task Detail',1
EXEC [dbo].pInsTranslation 'Department_could_not_be_updated',N'لا يمكن تحديث القسم','Department could not be updated','Department Detail',1
EXEC [dbo].pInsTranslation 'Department_Updated_Successfully',N'تم تحديث القسم بنجاح','Department Updated Successfully','Department Detail',1
EXEC [dbo].pInsTranslation 'Department_Added_Successfully',N'تم إضافة القسم بنجاح','Department Added Successfully','Department Detail',1
EXEC [dbo].pInsTranslation 'Could_not_create_a_new_Department',N'لا يمكن إنشاء قسم جديد','Could not create a new Department','Department Detail',1
EXEC [dbo].pInsTranslation 'Edit_Department_Detail',N'تعديل تفاصيل القسم','Edit Department Detail','Department Detail',1
EXEC [dbo].pInsTranslation 'Add_Legal_Legislation_Type',N'إضافة نوع التشريع','Add Legal Legislation Type','Legislation Detail',1
EXEC [dbo].pInsTranslation 'Edit_Legal_Legislation_Type',N'تعديل نوع التشريع','Edit Legal Legislation Type','Legislation Detail',1
EXEC [dbo].pInsTranslation 'Are_you_sure_that_you_want_to_delete_literature?',N'هل أنت متأكد من حذف الكتاب؟','Are you sure that you want to delete literature?','Literature Detail',1
 EXEC [dbo].pInsTranslation 'Document_Type',N'نوع المستند','Document Type','Document Type',1
EXEC [dbo].pInsTranslation 'Communication_Type',N'نوع التواصل','Communication Type','Communication Type',1
EXEC [dbo].pInsTranslation 'Communication_Type_(G2G_And_Fatwa)',N'نوع التواصل  (الفتوى والبوابة البينية)','Communication Type (G2G And Fatwa)','Communication Type',1
EXEC [dbo].pInsTranslation 'Case_File_Status',N'حالة ملف القضية','Case File Status','Case File Status',1
EXEC [dbo].pInsTranslation 'Case_Request_Status',N'حالة طلب القضية','Case Request Status','Case Request Status',1
 EXEC [dbo].pInsTranslation 'Case_Status',N'حالة القضية','Case Status','Case Status',1
 EXEC [dbo].pInsTranslation 'Task_Type',N'نوع المهمة','Task Type','Task Type',1
EXEC [dbo].pInsTranslation 'Government_Entities_(G2G_And_Fatwa)',N'الجهات الحكومية  (الفتوى والبوابة البينية)','Government Entities (G2G And Fatwa)','Government Entities',1
EXEC [dbo].pInsTranslation 'Court_Type',N'نوع المحكمة','Court Type','Court Type',1
EXEC [dbo].pInsTranslation 'Legal_Principle_Type',N'نوع المبدأ القانوني','Legal Principle Type','Legal Principle Type',1
EXEC [dbo].pInsTranslation 'Legal_Principle_Publication_Source',N'مصدر نشر المبدأ القانوني','Legal Principle Publication Source','LLS',1
EXEC [dbo].pInsTranslation 'LLS_Type',N'LLS Type','LLS Type','LLS Type',1
EXEC [dbo].pInsTranslation 'Are_you_Sure_You_Want_to_Delete_the_Government_Entity',N'هل أنت متأكد من أنك تريد حذف الجهة الحكومية؟' ,'Are you Sure You Want to Delete the Government Entity','Government Detail',1 
 EXEC [dbo].pInsTranslation 'Are_you_Sure_You_Want_to_Delete_Court_Type',N'هل أنت متأكد أنك تريد حذف نوع المحكمة','Are you Sure You Want to Delete Court Type','Court Detail',1 
EXEC [dbo].pInsTranslation 'Deleted_Successfully',N'تم الحذف بنجاح','Deleted Successfully','Delete Screen',1 
EXEC [dbo].pInsTranslation 'Government_Entity_Updated_Successfully',N'تم تحديث الجهة الحكومية بنجاح','Government Entity Updated Successfully','Government Entity',1 
EXEC [dbo].pInsTranslation 'Government_Entity_Added_Successfully',N'تم إضافة الجهة الحكومية بنجاح','Government Entity Added Successfully','Government Entity',1 
EXEC [dbo].pInsTranslation 'Communication_Type_Updated_Successfully',N'تم تحديث نوع التواصل بنجاح','Communication Type Updated Successfully','Communication Type',1 
EXEC [dbo].pInsTranslation 'Communication_type_could_not_be_updated',N'لا يمكن تحديث نوع التواصل','Communication type could not be updated','Communication Type',1 
EXEC [dbo].pInsTranslation 'Court_Type_could_not_be_updated',N'لا يمكن تحديث نوع المحكمة','Court Type could not be updated','Court Detail',1 
EXEC [dbo].pInsTranslation 'Court_Type_Updated_Successfully',N'تم تحديث نوع المحكمة بنجاح','Court Type Updated Successfully','Court Detail',1 
EXEC [dbo].pInsTranslation 'Could_not_create_a_new_Court_Type',N'لا يمكن إنشاء نوع محكمة جديد','Could not create a new Court Type','Court Detail',1 
EXEC [dbo].pInsTranslation 'Court_Type_Added_Successfully',N'تم إضافة نوع المحكمة بنجاح','Court Type Added Successfully','Court Detail',1 
EXEC [dbo].pInsTranslation 'Sector_Type',N'نوع القطاع','Sector Type','Sector Type',1 
EXEC [dbo].pInsTranslation 'Edit_Sector_Detail',N'تعديل تفاصيل القطاع','Edit Sector Detail','Sector Type',1 
EXEC [dbo].pInsTranslation 'Sector_Type_Updated_Successfully',N'تم تحديث نوع القطاع بنجاح','Sector Type Updated Successfully','Sector Type',1 
EXEC [dbo].pInsTranslation 'Sector_Type_could_not_be_updated',N'لا يمكن تحديث نوع القطاع','Sector Type could not be updated','Sector Type',1 
EXEC [dbo].pInsTranslation 'Add_Sector',N'إضافة القطاع','Add Sector','Sector Type',1 
EXEC [dbo].pInsTranslation 'Publication_Name_Id',N'رقم اسم النشر','Publication Name Id','Publication',1 
EXEC [dbo].pInsTranslation 'literature_Tags',N'وسوم الكتاب','literature Tags','literature',1 
EXEC [dbo].pInsTranslation 'Legal_Publication_Source',N'مصدر النشر القانوني','Legal Publication Source','literature',1
EXEC [dbo].pInsTranslation 'Chamber_Added_Successfully',N'تم إضافة الدائرة بنجاح','Chamber Added Successfully','literature',1 
EXEC [dbo].pInsTranslation 'Chamber_Updated_Successfully',N'تم تحديث الدائرة بنجاح','Chamber Updated Successfully','literature',1 
EXEC [dbo].pInsTranslation 'Case_Status_Upadated_Successfully',N'تم تحديث حالة القضية بنجاح','Case Status Updated Successfully','literature',1 
EXEC [dbo].pInsTranslation 'Add_Document_Type',N'إضافة نوع المستند','Add Document Type','literature',1 
EXEC [dbo].pInsTranslation 'Is_Official_Letter',N'هل يعد كتاب رسمي؟','Is Official Letter','literature',1 
EXEC [dbo].pInsTranslation 'Is_Ge_Portal_Type',N'هل النوع يتبع البوابة البينية؟','Is Ge Portal Type','literature',1 
EXEC [dbo].pInsTranslation 'Is_Mandatory',N'إلزامي','Is Mandatory','literature',1 
EXEC [dbo].pInsTranslation 'Tag_No',N'رقم الوسم','Tag No','Tag No',1 
EXEC [dbo].pInsTranslation 'Literature_Tags_Added_Successfully',N'تمت إضافة وسوم الكتاب بنجاح','Literature Tags Added Successfully','Tag No',1 
EXEC [dbo].pInsTranslation 'Could_not_create_a_new_Literature_Tags',N'لا يمكن إنشاء وسوم كتاب جديدة','Could not create a new Literature_Tags','Tag No',1 
EXEC [dbo].pInsTranslation 'Literature_Tags_Updated_Successfully',N'تم تحديث وسوم الكتاب بنجاح','Literature Tags Updated Successfully','Tag No',1 
EXEC [dbo].pInsTranslation 'Literature_Tags_could_not_be_updated',N'لا يمكن تحديث وسوم الكتاب','Literature Tags could not be updated','Tag No',1 
EXEC [dbo].pInsTranslation 'English_Name',N'الاسم انجليزي','English Name','English Name',1 
 EXEC [dbo].pInsTranslation 'Add_Legal_Principle_Type',N'إضافة نوع المبدأ القانوني','Add Legal Principle Type','Tag No',1 
EXEC [dbo].pInsTranslation 'Are_you_Sure_You_Want_to_Delete_Legal_Principle_Type',N'هل أنت متأكد أنك تريد حذف نوع المبدأ القانوني؟','Are you Sure You Want to Delete Legal Principle Type','Tag No',1 
EXEC [dbo].pInsTranslation 'Are_you_Sure_You_Want_to_Delete_Legal_Publication',N'هل أنت متأكد أنك تريد حذف النشر القانوني؟','Are you Sure You Want to Delete Legal Publication','Tag No',1 
EXEC [dbo].pInsTranslation 'Edit_Legal_Principle_Type',N'تعديل نوع المبدأ القانوني','Edit Legal Principle Type','Tag No',1 
EXEC [dbo].pInsTranslation 'Add_Publicaiton_Source',N'إضافة مصدر النشر','Add Publicaiton Source','Tag No',1 
EXEC [dbo].pInsTranslation 'Are_you_Sure_You_Want_to_Delete_Legal_Publication_Source_Name',N'هل أنت متأكد من أنك تريد حذف اسم مصدر النشر القانوني؟','Are you Sure You Want to Delete Legal Publication Source Name','Tag No',1 
EXEC [dbo].pInsTranslation 'Edit_Literature_Tag',N'تعديل وسوم الكتاب','Edit Literature Tag','Tag No',1 
EXEC [dbo].pInsTranslation 'Are_you_Sure_You_Want_to_Delete_Literature_Tag',N'هل أنت متأكد أنك تريد حذف وسم الكتاب؟','Are you Sure You Want to Delete Literature Tag','Tag No',1 
EXEC [dbo].pInsTranslation 'Add_Literature_Tag',N'إضافة وسم الكتاب','Add Literature Tag','Tag No',1 
EXEC [dbo].pInsTranslation 'Arabic_Name',N'الاسم عربي','Arabic Name','Tag No',1 
 EXEC [dbo].pInsTranslation 'Legal_Principle_Type_Added_Successfully',N'تم إضافة نوع المبدأ القانوني بنجاح','Legal Principle Type Added Successfully','Tag No',1 
EXEC [dbo].pInsTranslation 'legal_Principle_Type_could_not_be_updated',N'لا يمكن تعديل نوع المبدأ القانوني','legal Principle Type could not be updated','Tag No',1 
EXEC [dbo].pInsTranslation 'Legal_Principle_Type_Updated_Successfully',N'تم تعديل نوع المبدأ القانوني بنجاح','Legal Principle Type Updated Successfully','Tag No',1 
EXEC [dbo].pInsTranslation 'Please_Select_Year_Pattern_First',N'يرجى اختيار نمط السنة','Please Select Year Pattern First','Tag No',1 
EXEC [dbo].pInsTranslation 'Please_Select_Month_Pattern_First',N'يرجى اختيار نمط الشهر','Please Select Month Pattern First','Tag No',1 
 

 EXEC [dbo].pInsTranslation 'This_Sequence_is_already_Selected_Please_Select_Other_Sequence',N'تم اختيار رقم التسلسل مسبقا، يرجى اختيار رقم تسلسل آخر','This Sequence is already Selected Please Select Other Sequence','Tag No',1
 EXEC [dbo].pInsTranslation 'Select_Enter_Charater_String_First',N'يرجى اختيار وادخال الأحرف','Select Enter Charater String First','Tag No',1 
EXEC [dbo].pInsTranslation 'Please_Select_Pattern_Day',N'يرجى اختيار نمط اليوم','Please Select Pattern Day','Tag No',1 
EXEC [dbo].pInsTranslation 'Designation_of_Representative',N'المسمى الوظيفي لممثل الجهة الحكومية','Designation of Representative','Tag No',1 
EXEC [dbo].pInsTranslation 'Time_Intervals',N'المدد الزمنية','Time Intervals','Tag No',1 
EXEC [dbo].pInsTranslation 'Time_Interval_Add',N'إضافة المدة الزمنية','Time Interval Add','Tag No',1 
EXEC [dbo].pInsTranslation 'Is_NotificationGenerate',N'Is NotificationGenerate','Is NotificationGenerate','Tag No',1 
EXEC [dbo].pInsTranslation 'Edit_Time_Interval',N'تعديل المدة الزمنية','Edit Time Interval','Tag No',1
 EXEC [dbo].pInsTranslation 'Interval_Added_Successfully',N'تم إضافة المدة الزمنية بنجاح','Interval Added Successfully','Tag No',1
EXEC [dbo].pInsTranslation 'Could_not_create_a_new_interval',N'لا يمكن إنشاء مدة زمنية جديدة','Could not create a new interval','Tag No',1
EXEC [dbo].pInsTranslation 'Interval_Updated_Successfully',N'تم تعديل المدة الزمنية بنجاح','Interval Updated Successfully','Tag No',1
 EXEC [dbo].pInsTranslation 'Add_Number_Patterns',N'إضافة نمط رقم التسلسل','Add Number Patterns','Number Pattern',1
EXEC [dbo].pInsTranslation 'Number_Type',N'نوع رقم التسلسل','Sequence Number Type','Number Pattern',1
EXEC [dbo].pInsTranslation 'Set_Order_Sequence',N'ضبط ترتيب رقم التسلسل','Set Order Sequence','Number Pattern',1
EXEC [dbo].pInsTranslation 'Static_Text_Pattern',N'نمط النص','Static Text Pattern','Number Pattern',1
EXEC [dbo].pInsTranslation 'Sequence_Number',N'رقم التسلسل','Sequence Number','Number Pattern',1
EXEC [dbo].pInsTranslation 'Is_Update_Year',N'إعادة تعيين رقم التسلسل كل سنة','Reset Sequence each Year','Number Pattern',1
EXEC [dbo].pInsTranslation 'Preview_Sequence',N'عرض رقم التسلسل','Preview Sequence','Number Pattern',1
EXEC [dbo].pInsTranslation 'Numbers_Pattern_Setup',N'ضبط نمط رقم التسلسل','Numbers Pattern Setup','Number Pattern',1
EXEC [dbo].pInsTranslation 'Sequence_Result',N'نتيجة رقم التسلسل','Sequence Number Result','Number Pattern',1
EXEC [dbo].pInsTranslation 'Pattern_Name',N'اسم النمط','Pattern Name','Number Pattern',1
EXEC [dbo].pInsTranslation 'Number_Patterns',N'أنماط رقم التسلسل','Sequence Number Patterns','Number Pattern',1
 EXEC [dbo].pInsTranslation 'Is_Default',N'رقم التسلسل الافتراضي','Is Default','Number Pattern',1
EXEC [dbo].pInsTranslation 'Number_Patterns_History',N'تغييرات رقم التسلسل','Number Patterns History','Number Pattern',1    
EXEC [dbo].pInsTranslation 'Edit_Number_Patterns',N'تعديل نمط رقم التسلسل','Edit Sequence Number Patterns','Number Pattern',1    
EXEC [dbo].pInsTranslation 'Are_you_Sure_You_Want_to_Delete_Pattern',N'هل أنت متأكد أنك تريد حذف النمط؟','Are you Sure You Want to Delete Pattern?','Number Pattern',1    
EXEC [dbo].pInsTranslation 'Are_you_Sure_you_want_to_update_Status',N'هل أنت متأكد أنك تريد تعديل الحالة؟','Are you Sure you want to update Status','Number Pattern',1    
EXEC [dbo].pInsTranslation 'Updated_Successfully',N'تم التعديل بنجاح','Updated_Successfully','Number Pattern',1    
EXEC [dbo].pInsTranslation 'Could_not_create_a_new_Case_file_number',N'لا يمكن إضافة رقم ملف قضية جديد','Could not create a new Case file number','Number Pattern',1    
EXEC [dbo].pInsTranslation 'Pattern_Added_Successfully',N'تم إضافة النمط بنجاح','Pattern Added Successfully','Number Pattern',1    
EXEC [dbo].pInsTranslation 'Pattern_could_not_be_updated',N'لا يمكن تعديل النمط','Could not update the pattern','Number Pattern',1    
EXEC [dbo].pInsTranslation 'Pattern_Updated_Successfully',N'تم تعديل النمط بنجاح','Pattern Updated Successfully','Number Pattern',1   
EXEC [dbo].pInsTranslation 'Government_Entity',N'الجهات الحكومية','Government_Entity','Number Pattern',1   
EXEC [dbo].pInsTranslation 'Government_Entity_Already_Exist',N'تم إضافة رقم التسلسل لهذه الجهة الحكومية مسبقا','Government Entity Already Exist','Number Pattern',1 
EXEC [dbo].pInsTranslation 'Case_Consultation_Number_Pattern',N'نمط رقم التسلسل لملفات القضايا/الاستشاري','Case/Consultation Sequence Number Pattern','Number Pattern',1 
EXEC [dbo].pInsTranslation 'Case_Request_Number_Pattern',N'نمط رقم التسلسل لطلبات القضايا','Case Request Number Pattern','Number Pattern',1 
EXEC [dbo].pInsTranslation 'Case_File_Number_Pattern',N'نمط رقم التسلسل لملفات القضايا','Case File Number Pattern','Number Pattern',1 
EXEC [dbo].pInsTranslation 'Consultation_File_Number_Pattern',N'نمط رقم التسلسل لملفات الاستشاري','Consultation File Number Pattern','Number Pattern',1 
EXEC [dbo].pInsTranslation 'Inbox_Number_Pattern',N'نمط رقم التسلسل لرقم الوارد','Inbox Number Pattern','Number Pattern',1 
EXEC [dbo].pInsTranslation 'Outbox_Number_Pattern',N'نمط رقم التسلسل لرقم الصادر','Outbox Number Pattern','Number Pattern',1 
EXEC [dbo].pInsTranslation 'Consultation_Number_Pattern',N'نمط رقم التسلسل لطلبات الاستشاري','Consultation Number Pattern','Number Pattern',1 
EXEC [dbo].pInsTranslation 'Please_Select_Character_String_Pattern_First',N'يرجى اختيار نمط الأحرف','Please Select Character String Pattern First','Number Pattern',1 
EXEC [dbo].pInsTranslation 'Please_Select_Sequence_Number_Pattern_First',N'يرجى اختيار نمط رقم التسلسل','Please Select Sequence Number Pattern First','Number Pattern',1 
EXEC [dbo].pInsTranslation 'Edit_Publicaiton_Source',N'تعديل مصدر النشر','Edit Publicaiton Source','Tag No',1 
EXEC [dbo].pInsTranslation 'Legislation_Type_Added_Successfully',N'تم إضافة نوع التشريع بنجاح','Legislation Type Added Successfully','Lookups',1
EXEC [dbo].pInsTranslation 'Legislation_Type_Updated_Successfully',N'تم تعديل نوع التشريع بنجاح','Legislation Type Updated Successfully','Lookups',1
EXEC [dbo].pInsTranslation 'Courts_Details_(G2G_And_Fatwa)',N'تفاصيل المحكمة (الفتوى والبوابة البينية)','Courts Details (G2G And Fatwa)','Courts Details',1 
EXEC [dbo].pInsTranslation 'Chamber_(G2G_And_Fatwa)',N'الدائرة (الفتوى والبوابة البينية)','Chamber (G2G And Fatwa)','Chamber Details',1 
 EXEC [dbo].pInsTranslation 'Request_Type_(G2G_And_Fatwa)',N'نوع الطلب (الفتوى والبوابة البينية)','Request Type (G2G And Fatwa)','Request Type Details',1 
EXEC [dbo].pInsTranslation 'Coms_International_Arbitration_(G2G_And_Fatwa)',N'أنواع ملف التحكيم الدولي (الفتوى والبوابة البينية)','Coms International Arbitration (G2G And Fatwa)',' Consultation detail',1 
EXEC [dbo].pInsTranslation 'Consultation_Legislation_File_Type_(G2G_And_Fatwa)',N'أنواع ملف التشريع (الفتوى والبوابة البينية)','Consultation Legislation File Type (G2G And Fatwa)','Consultation Legislation',1 
EXEC [dbo].pInsTranslation 'Chambers_Number',N'أرقام الدوائر','Chambers Number','Chambers Number',1
EXEC [dbo].pInsTranslation 'Add_Chamber_Number_Detail',N'إضافة تفاصيل رقم الدائرة','Add Chamber Number Detail','Chambers Number',1
EXEC [dbo].pInsTranslation 'Edit_Chamber_Number_Detail',N'تعديل تفاصيل رقم الدائرة','Edit Chamber Number Detail','Chambers Number',1
EXEC [dbo].pInsTranslation 'Are_you_Sure_You_Want_to_Delete_Chamber_Number',N'هل أنت متأكد من حذف رقم الدائرة؟','Are you Sure You Want to Delete Chamber Number?','Chambers Number',1
EXEC [dbo].pInsTranslation 'Could_not_create_a_new_Chamber_Number',N'لا يمكن إنشاء رقم دائرة جديدة','Could not create a new Chamber Number','Chambers Number',1
EXEC [dbo].pInsTranslation 'Chamber_Number_Added_Successfully',N'تم إضافة رقم الدائرة بنجاح','Chamber Number Added Successfully','Chambers Number',1
EXEC [dbo].pInsTranslation 'Chamber_Number_could_not_be_updated',N'لا يمكن تعديل رقم الدائرة','Chamber Number could not be updated','Chambers Number',1
EXEC [dbo].pInsTranslation 'Chamber_Number_Updated_Successfully',N'تم تعديل رقم الدائرة بنجاح','Chamber Number Updated Successfully','Chambers Number',1
EXEC [dbo].pInsTranslation 'Add_Publication_Source',N'إضافة مصدر النشر','Add Publication Source','Publication',1 
EXEC [dbo].pInsTranslation 'Edit_Publication_Source',N'تعديل مصدر النشر','Edit Publication Source','publication',1 
EXEC [dbo].pInsTranslation 'Module_Name',N'اسم الوحدة','Module Name','DocumentType',1 
EXEC [dbo].pInsTranslation 'Edit_Legislation_File_Type',N'تعديل نوع ملف التشريع','Edit Legislation File Type','DocumentType',1 
EXEC [dbo].pInsTranslation 'Add_Legislation_File_Type',N'إضافة نوع ملف التشريع','Add Legislation File Type','DocumentType',1 
EXEC [dbo].pInsTranslation 'Communication_Type_Updated_Successfully',N'تم تعديل نوع التواصل بنجاح','Communication Type Updated Successfully','Communication Type',1
EXEC [dbo].pInsTranslation 'Edit_Literature_Tag',N'تعديل وسم الكتاب','Edit Literature Tag','Communication Type',1
EXEC [dbo].pInsTranslation 'Legal_Publication_Source_could_not_be_updated',N'لا يمكن تعديل مصدر نشر التشريع','Legal Publication Source could not be updated','Communication Type',1
EXEC [dbo].pInsTranslation 'Successfully_edited_legal_publication_source',N'تم تعديل مصدر نشر التشريع بنجاح','Legislation Publication Source Updated Successfully','Communication Type',1
EXEC [dbo].pInsTranslation 'Successfully_added_legal_publication_source',N'تم إضافة مصدر نشر التشريع بنجاح','Legislation Publication Source Added Successfully','Communication Type',1
 EXEC [dbo].pInsTranslation 'Edit_Legal_Principle_Publication_Source',N'تعديل مصدر نشر المبدأ القانوني','Edit Legal Principle Publication Source','DocumentType',1 
 EXEC [dbo].pInsTranslation 'Add_Legal_Principle_Publication_Source',N'إضافة مصدر نشر المبدأ القانوني','Add Legal Principle Publication Source','DocumentType',1 
 EXEC [dbo].pInsTranslation 'Are_you_Sure_You_Want_to_Delete_Legal_Principle_Publication_Source',N'هل أنت متأكد أنك تريد حذف مصدر نشر المبدأ القانوني؟','Are you Sure You Want to Delete Legal Principle Publication Source?','DocumentType',1 
 EXEC [dbo].pInsTranslation 'Static_Text_Pattern',N'نمط النص','Static Text Pattern','Number Pattern',1
 EXEC [dbo].pInsTranslation 'Reset_Yearly',N'إعادة التعيين سنويا','Reset Yearly','Number Pattern',1
  EXEC [dbo].pInsTranslation 'Communication_Type_(G2G_And_Fatwa)',N'نوع التواصل  (الفتوى والبوابة البينية)','Communication Type (G2G And Fatwa)','Number Pattern',1
 EXEC [dbo].pInsTranslation 'Government_Entities_(G2G_And_Fatwa)',N'الجهات الحكومية  (الفتوى والبوابة البينية)','Government Entities (G2G And Fatwa)','Number Pattern',1
 EXEC [dbo].pInsTranslation 'Preview_Pattern',N'عرض النمط','Preview Pattern','Number Pattern',1  
EXEC [dbo].pInsTranslation 'Edit_International_Arbitration',N'تعديل التحكيم الدولي','Edit International Arbitration','Number Pattern',1 
EXEC [dbo].pInsTranslation 'Add_International_Arbitration',N'إضافة التحكيم الدولي','Add International Arbitration','Number Pattern',1 
EXEC [dbo].pInsTranslation 'Edit_Request_Detail',N'تعديل تفاصيل الطلب','Edit Request Detail','Number Pattern',1 
EXEC [dbo].pInsTranslation 'Government_History',N'تغييرات الجهات الحكومية','Government History','Number Pattern',1
EXEC [dbo].pInsTranslation 'Are_you_Sure_You_Want_to_Delete_Government_Entity_sector',N'هل أنت متأكد أنك تريد حذف قطاع الجهة الحكومية؟','Are you Sure You Want to Delete Government Entity sector?','Number Pattern',1 
EXEC [dbo].pInsTranslation 'Are_you_Sure_You_Want_to_Delete_Government_Entity_Representative',N'هل أنت متأكد أنك تريد حذف ممثل الجهة الحكومية؟','Are you Sure You Want to Delete Government Entity Representative?','Number Pattern',1 
EXEC [dbo].pInsTranslation 'Edit_GE_Representative_Detail',N'تعديل تفاصيل ممثل الجهة الحكومية','Edit GE Representative Detail','Number Pattern',1 
EXEC [dbo].pInsTranslation 'Add_GE_Representative_Detail',N'إضافة تفاصيل ممثل الجهة الحكومية','Add GE Representative Detail','Number Pattern',1 
EXEC [dbo].pInsTranslation 'Common_Lookups',N'قوائم شائعة الاستخدام','Common Lookups','Number Pattern',1 
EXEC [dbo].pInsTranslation 'Government_Entities_Representatives',N'ممثلي الجهات الحكومية','Government Entities Representatives','Number Pattern',1 
EXEC [dbo].pInsTranslation 'Government_Entities_Sectors',N'قطاعات الجهات الحكومية','Government Entities Sectors','Number Pattern',1 
EXEC [dbo].pInsTranslation 'Legislation_File_Type_Added_Successfully',N'تم إضافة نوع ملف التشريع بنجاح','Legislation File Type Added Successfully','Legislation File',1
EXEC [dbo].pInsTranslation 'Legislation_File_Type_Updated_Successfully',N'تم تعديل نوع ملف التشريع بنجاح','Legislation File Type Updated Successfully','Legislation File',1
EXEC [dbo].pInsTranslation 'This_Sequence_Number_Already_Selected',N'This Sequence Number Already Selected','This Sequence Number Already Selected','Number Pattern',1
EXEC [dbo].pInsTranslation 'Are_You_Sure_You_Want_To_Delete_Legislation_Type',N'Are You Sure You Want To Delete Legislation Type','Are You Sure You Want To Delete Legislation Type','Legislation File',1
----------------------------16-01-2024-------------
EXEC [dbo].pInsTranslation 'reset_password_for',N'هل أنت متأكد أنك تريد إعادة تعيين كلمة المرور','Are you sure you want to reset password for','Reset Password',1
EXEC [dbo].pInsTranslation 'Set_Temporary_Password',N'تعيين كلمة مرور المستخدم مؤقتًا','Set Temporary Password','Reset Password',1
EXEC [dbo].pInsTranslation 'Reset_Password',N'إعادة تعيين كلمة المرور','Reset Password','Edit/Update User',1
EXEC [dbo].pInsTranslation 'Employee_Id',N'رقم ID الموظف','Employee Id ','List Employee',1
EXEC [dbo].pInsTranslation 'Sector_Type',N'نوع قطاع التشغيل','Sector Type','List Employee',1
EXEC [dbo].pInsTranslation 'Sender_Name',N'اسم المرسل','Sender Name','dashboard',1
EXEC [dbo].pInsTranslation 'Sent_By',N'مرسل من قبل','Send By','dashboard',1
EXEC [dbo].pInsTranslation 'Sent_To',N'مرسلة إلى','Send To','dashboard',1
EXEC [dbo].pInsTranslation 'Transaction_Date',N'تاريخ العملية','Transaction Date','dashboard',1
EXEC [dbo].pInsTranslation 'Activity',N'نشاط','Activity','dashboard',1
EXEC [dbo].pInsTranslation 'Task_Number',N'رقم المهمة','Task Number','task details screen',1
EXEC [dbo].pInsTranslation 'Pending',N'قيد الانتظار','Pending','task details screen',1
EXEC [dbo].pInsTranslation 'Completed',N'مكتمل','Completed','task details screen',1
EXEC [dbo].pInsTranslation 'Activity',N'نشاط','Activity','task details screen',1


EXEC [dbo].pInsTranslation 'Assign_Lawyer_To_Courts',N'تعيين الأعضاء للمحاكم','Assign Lawyers To Courts','CMS',1
EXEC [dbo].pInsTranslation 'Court',N'المحكمة','Court','CMS',1
EXEC [dbo].pInsTranslation 'Chamber',N'الدائرة','Chamber','CMS',1
EXEC [dbo].pInsTranslation 'Chamber_Number',N'رقم الدائرة','Chamber Number','CMS',1

------Ammaar Naveed-----16/01/2024---------
EXEC [dbo].pInsTranslation 'Employee_Id',N'رقم ID الموظف','Employee Id','Add Employee Dialog',1
EXEC [dbo].pInsTranslation 'Passport_Number',N'رقم جواز السفر','Passport Number','Add Employee',1
EXEC [dbo].pInsTranslation 'Employee_Status',N'حالة الموظف','Employee Status','Add Employee',1
EXEC [dbo].pInsTranslation 'Group_Type',N'نوع المجموعة','Group Type','Add Employee',1
EXEC [dbo].pInsTranslation 'Governorate',N'المحافظة','Governorate','Add Employee',1
EXEC [dbo].pInsTranslation 'Address',N'العنوان','Address','Add Employee',1
UPDATE tTranslation SET Value_En='Date of Joining' WHERE TranslationKey='Date_Of_Joining' 
EXEC [dbo].pInsTranslation 'Resigned/Terminated_Date',N'تاريخ الاستقالة/إنهاء الخدمة','Resigned/Terminated Date','Add Employee',1
EXEC [dbo].pInsTranslation 'Educational_Information',N'معلومات التعليم','Educational Information','Add Employee',1
EXEC [dbo].pInsTranslation 'Trainings',N'التدريب','Trainings','Add Employee',1
EXEC [dbo].pInsTranslation 'University_Name',N'اسم الجامعة','University Name','Add Employee',1
EXEC [dbo].pInsTranslation 'Majoring_Name',N' اسم التخصص','Majoring Name','Add Employee',1
EXEC [dbo].pInsTranslation 'Percentage/Grade',N'النسبة المئوية/الدرجة','Percentage/Grade','Add Employee',1
EXEC [dbo].pInsTranslation 'Add_Education',N'أضف التعليم','Add Education','Add Employee',1
EXEC [dbo].pInsTranslation 'Job_Title',N'المسمى الوظيفي','Job Title','Add Employee',1
EXEC [dbo].pInsTranslation 'Job_Experience',N'خبرة العمل','Job Experience','Add Employee',1
EXEC [dbo].pInsTranslation 'End_Date',N'تاريخ النهاية','End Date','Add Employee',1
EXEC [dbo].pInsTranslation 'Training_Name',N'اسم التدريب','Training Name','Add Employee',1
EXEC [dbo].pInsTranslation 'Training_Center_Name',N'اسم مركز التدريب','Training Center Name','Add Employee',1
EXEC [dbo].pInsTranslation 'Add_Training',N'أضف التدريب','Add Training','Add Employee',1
EXEC [dbo].pInsTranslation 'Training_Center_Location',N'موقع مركز التدريب','Training Center Location','Add Employee',1
EXEC [dbo].pInsTranslation 'Deactivation_Reason',N'سبب التعطيل','Deactivation Reason','Add Employee',1
EXEC [dbo].pInsTranslation 'deactivate_confirm_message',N'هل أنت متأكد من أنك تريد تعطيل حساب هذا المستخدم؟','Are you sure you want to deactivate this employee?','Deactivate Employee',1
EXEC [dbo].pInsTranslation 'Employment_Information',N'معلومات التوظيف','Employment Information','View Employee',1
EXEC [dbo].pInsTranslation 'Education_Information',N'معلومات التعليم','Educational Information','View Employee',1
EXEC [dbo].pInsTranslation 'Employee_Detail',N'تفاصيل الموظف','Employee Details','View Employee',1
EXEC [dbo].pInsTranslation 'Request_Detail',N'تفاصيل الطلب','Request Details','View Employee',1
UPDATE tTranslation SET Value_En='Logout' WHERE TranslationKey='LogOut'
UPDATE tTranslation SET Value_En='Internal Employees' WHERE TranslationKey='Internal_Employee'
UPDATE tTranslation SET Value_En='External Employees' WHERE TranslationKey='External_Employee'

------Ammaar Naveed-----17/01/2024---------
EXEC [dbo].pInsTranslation 'Edit_External_Employee',N'تعديل الموظف الخارجي','Edit External Employee','Edit Employee',1
EXEC [dbo].pInsTranslation 'Edit_Internal_Employee',N'تعديل الموظف الداخلي','Edit Internal Employee','Edit Employee',1
EXEC [dbo].pInsTranslation 'Employee_Name',N'تعديل الموظف','Employee Name','View Employee',1
EXEC [dbo].pInsTranslation 'Import_Bulk_Employee',N'استيراد بالجملة للموظفين','Bulk Import Employees','Add Employee',1
EXEC [dbo].pInsTranslation 'Export_Template',N'إصدار قالب','Export Template','Add Employee',1
EXEC [dbo].pInsTranslation 'Download_Template',N'تنزيل قالب','Download Template','Add Employee',1
EXEC [dbo].pInsTranslation 'Import_Template',N'استيراد القلب','Import Template','Add Employee',1
EXEC [dbo].pInsTranslation 'Area',N'منطقة','Area','Add Employee',1
EXEC [dbo].pInsTranslation 'Group',N'مجموعة','Group','Add Employee',1
EXEC [dbo].pInsTranslation 'Grade',N'درجة','Grade','Add Employee',1
EXEC [dbo].pInsTranslation 'Work_Experience',N'خبرة العمل','Work Experience','Add Employee',1
EXEC [dbo].pInsTranslation 'Additional_Information',N'معلومات إضافية','Additional Information','Add/Edit Employee',1
EXEC [dbo].pInsTranslation 'View_Employee_Detail',N'عرض تفاصيل الموظف','View Employee Details','View Employee Detail',1
EXEC [dbo].pInsTranslation 'Deactivate_User',N'إلغاء تنشيط الموظف','Deactivate Employee','Edit Employee',1
EXEC [dbo].pInsTranslation 'Graduation_Year',N'سنة التخرج','Graduation Year','Add/Edit Employee',1
EXEC [dbo].pInsTranslation 'enter_complete_address',N'إدخال عنوان الجامعة بالكامل','Enter Complete University Address','Add/Edit Employee',1
EXEC [dbo].pInsTranslation 'University_Address',N'عنوان الجامعة','University Address','Add/Edit Employee',1
EXEC [dbo].pInsTranslation 'AD_UserName',N'اسم المستخدم Active Directory','Active Directory Username','View Employee Details',1
---------------
EXEC [dbo].pInsTranslation 'Assign_Lawyer_To_Courts',N'تعيين الأعضاء للمحاكم','Assign Lawyers To Courts','CMS',1
EXEC [dbo].pInsTranslation 'Court',N'المحكمة','Court','CMS',1
EXEC [dbo].pInsTranslation 'Chamber',N'الدائرة','Chamber','CMS',1
EXEC [dbo].pInsTranslation 'Chamber_Number',N'رقم الدائرة','Chamber Number','CMS',1

----------12-01-2024-------
EXEC [dbo].pInsTranslation 'Employee_Id',N'رقم ID الموظف','Employee Id','Add Employee',1
EXEC [dbo].pInsTranslation 'Login_Id',N'رقم تسجيل الدخول','Login Id','Add Employee',1
------Ammaar Naveed-----18/01/2024---------
EXEC [dbo].pInsTranslation 'Contract_Type',N'نوع العقد','Contract Type','Add Employee',1
UPDATE tTranslation SET Value_En='Internal Employee' WHERE TranslationKey='Internal_Employee'
UPDATE tTranslation SET Value_En='External Employee' WHERE TranslationKey='External_Employee'
EXEC [dbo].pInsTranslation 'Internal_Employees',N'الموظفين الداخليين','Internal Employees','Add/List Employee',1
EXEC [dbo].pInsTranslation 'External_Employees',N'الموظفين الخارجيين','External Employees','Add/List Employee',1
EXEC [dbo].pInsTranslation 'Edit_Employee',N'تعديل الموظف','Edit Employee','Add/List Employee',1
------Ammaar Naveed-----22/01/2024---------
EXEC [dbo].pInsTranslation 'Add_Education',N'أضف التعليم','Add Education','Add/Edit Employee',1
EXEC [dbo].pInsTranslation 'Add_Training',N'أضف التدريب','Add Training','Add/Edit Employee',1
EXEC [dbo].pInsTranslation 'Add_Work_Experience',N'أضف تجربة العمل','Add Work Experience','Add/Edit Employee',1
EXEC [dbo].pInsTranslation 'Additional_Info_Null_Message',N'يرجى ملئ التفاصيل أولاً','Please fill out the details first.','Add/Edit Employee',1
EXEC [dbo].pInsTranslation 'Tenure_Experience',N'الحيازة/الخبرة','Tenure/Experience','Add/Edit Employee',1

---------18-01-2024----------
EXEC [dbo].pInsTranslation 'Training_Center_Location',N'موقع مركز التدريب','Training Center Location','Add Employee',1
EXEC [dbo].pInsTranslation 'FirstName_En', N'الاسم الأول (انجليزي)', 'First Name (English)', 'Add Employee', 1;
EXEC [dbo].pInsTranslation 'SecondName_En', N'الاسم الثاني (انجليزي)', 'Second Name (English)', 'Add Employee', 1;
EXEC [dbo].pInsTranslation 'SecondName_Ar', N'الاسم الثاني (عربي)', 'Second Name (Arabic)', 'Add Employee', 1;
EXEC [dbo].pInsTranslation 'LastName_Ar', N'الاسم الأخير (عربي)', 'Last Name (Arabic)', 'Add Employee', 1;
EXEC [dbo].pInsTranslation 'FirstName_Ar', N'الاسم الأول (عربي)', 'First Name (Arabic)', 'Add Employee', 1;


------Ammaar Naveed-----18/01/2024---------
EXEC [dbo].pInsTranslation 'Contract_Type',N'نوع العقد','Contract Type','Add Employee',1
UPDATE tTranslation SET Value_En='Internal Employee' WHERE TranslationKey='Internal_Employee'
UPDATE tTranslation SET Value_En='External Employee' WHERE TranslationKey='External_Employee'
EXEC [dbo].pInsTranslation 'Internal_Employees',N'الموظفين الداخليين','Internal Employees','Add/List Employee',1
EXEC [dbo].pInsTranslation 'External_Employees',N'الموظفين الخارجيين','External Employees','Add/List Employee',1
EXEC [dbo].pInsTranslation 'Edit_Employee',N'تعديل الموظف','Edit Employee','Add/List Employee',1
------Ammaar Naveed-----22/01/2024---------
EXEC [dbo].pInsTranslation 'Add_Education',N'أضف التعليم','Add Education','Add/Edit Employee',1
EXEC [dbo].pInsTranslation 'Add_Training',N'أضف التدريب','Add Training','Add/Edit Employee',1
EXEC [dbo].pInsTranslation 'Add_Work_Experience',N'أضف تجربة العمل','Add Work Experience','Add/Edit Employee',1
EXEC [dbo].pInsTranslation 'Additional_Info_Null_Message',N'يرجى ملئ التفاصيل أولاً','Please fill out the details first.','Add/Edit Employee',1
EXEC [dbo].pInsTranslation 'Tenure_Experience',N'الحيازة/الخبرة','Tenure/Experience','Add/Edit Employee',1
------Ammaar Naveed-----23/01/2024---------
EXEC [dbo].pInsTranslation 'Welcome',N'مرحباً','Welcome','MainLayout',1
UPDATE tTranslation SET Value_En='The start date should not be greater than end date.' WHERE TranslationKey='FromDate_NotGreater_ToDate'
EXEC [dbo].pInsTranslation 'Wrong_Date_Of_Joining_Message',N'يجب ان يكون تاريخ بداية العمل اقل من تاريخ التعيين','The start date of work experience should be less than date of joining. ','Add/Edit Employee',1
------Ammaar Naveed-----24/01/2024---------
EXEC [dbo].pInsTranslation 'password_success_message',N'تم إعادة تعيين كلمة المرور بنجاح','Password has been successfully reset for','ResetPassword',1

------Ammaar Naveed-----25/01/2024---------Translations for Modules and Submodules Headings
EXEC [dbo].pInsTranslation 'Workflow',N'آلية العمل','Workflow','FATWA Admin Permissions Page',1
EXEC [dbo].pInsTranslation 'List_Of_Workflows',N'قائمة آلية العمل','List of Workflows','FATWA Admin Permissions Page',1
EXEC [dbo].pInsTranslation 'Workflow',N'آلية العمل','Workflow','FATWA Admin Permissions Page',1
EXEC [dbo].pInsTranslation 'User_Management',N'إدارة المستخدم','User Management','FATWA Admin Permissions Page',1
EXEC [dbo].pInsTranslation 'Menu',N'القائمة','Menu','FATWA Admin Permissions Page',1
EXEC [dbo].pInsTranslation 'Timelog',N'سجل الوقت','Time Log','FATWA Admin Permissions Page',1
EXEC [dbo].pInsTranslation 'Meeting',N'اجتماع','Meeting','FATWA Admin Permissions Page',1
EXEC [dbo].pInsTranslation 'Legal_Principles_Management_System',N'نظام إدارة المبادئ القانونية','Legal Principles Management System','FATWA Admin Permissions Page',1
EXEC [dbo].pInsTranslation 'Principles',N'المبادئ','Principles','FATWA Admin Permissions Page',1
EXEC [dbo].pInsTranslation 'LPS_Principle_Tag',N'وسم المبدأ القانوني','Legal Principle Tag','FATWA Admin Permissions Page',1
EXEC [dbo].pInsTranslation 'Masked_Principles',N'مبادئ مظللة','Masked Principles','FATWA Admin Permissions Page',1
EXEC [dbo].pInsTranslation 'Inventory_Management',N'إدارة المخازن','Inventory Management','FATWA Admin Permissions Page',1
EXEC [dbo].pInsTranslation 'Requests_List',N'قائمة الطلبات','Requests List','FATWA Admin Permissions Page',1
EXEC [dbo].pInsTranslation 'Request_Item_Detail',N'طلب تفاصيل العنصر','Request Item Details','FATWA Admin Permissions Page',1
EXEC [dbo].pInsTranslation 'Inventory',N'المخازن','Inventory','FATWA Admin Permissions Page',1
EXEC [dbo].pInsTranslation 'Library_Management_System',N'نظام إدارة المكتبة','Library Management System','FATWA Admin Permissions Page',1
EXEC [dbo].pInsTranslation 'Literatures',N'الكتب','Literatures','FATWA Admin Permissions Page',1
EXEC [dbo].pInsTranslation 'Literature_Borrow_Detail',N'تفاصيل استعارة الكتاب','Literature Borrow Detail','FATWA Admin Permissions Page',1
EXEC [dbo].pInsTranslation 'Legislation_Management_System',N'نظام إدارة التشريع','Legislation Management System','FATWA Admin Permissions Page',1
EXEC [dbo].pInsTranslation 'FATWA_Dashboard',N'لوحة المعلومات لبوابة الفتوى','FATWA Dashboard','FATWA Admin Permissions Page',1
EXEC [dbo].pInsTranslation 'Case_Management_System',N'نظام إدارة القضايا','Case Management System','FATWA Admin Permissions Page',1
EXEC [dbo].pInsTranslation 'Consultation_Management_System',N'نظام إدارة الاستشاري','Consultation Management System','FATWA Admin Permissions Page',1
EXEC [dbo].pInsTranslation 'Workflows',N'آليات العمل','Workflows','FATWA Admin Permissions Page',1
EXEC [dbo].pInsTranslation 'Employees',N'الموظفين','Employees','FATWA Admin Permissions Page',1
EXEC [dbo].pInsTranslation 'Create_Contact',N'إنشاء عقود','Create Contacts','FATWA Admin Permissions Page',1
EXEC [dbo].pInsTranslation 'Delete_Contact',N'حذف العقود','Delete Contacts','FATWA Admin Permissions Page',1
EXEC [dbo].pInsTranslation 'Kuwait_Al_Yawm',N'الكويت اليوم','Kuwait Al-Yawm','FATWA Admin Permissions Page',1
EXEC [dbo].pInsTranslation 'Consultation_Management_System',N'نظام إدارة ملفات الاستشاري','Consultation Management System','FATWA Admin Permissions Page',1
EXEC [dbo].pInsTranslation 'Draft_Document',N'مسودة المستند','Draft Document','FATWA Admin Permissions Page',1
EXEC [dbo].pInsTranslation 'Inbox_Outbox',N'الوارد/الصادر','Inbox/Outbox','FATWA Admin Permissions Page',1
EXEC [dbo].pInsTranslation 'Communication_Details',N'تفاصيل التواصل','Communication Details','FATWA Admin Permissions Page',1
EXEC [dbo].pInsTranslation 'Case_File',N'ملفات القضايا','Case Files','FATWA Admin Permissions Page',1
EXEC [dbo].pInsTranslation 'Registered_Case',N'القضايا المسجلة','Registered Cases','FATWA Admin Permissions Page',1
EXEC [dbo].pInsTranslation 'Case_Draft',N'مسودة المستندات الخاصة بالقضايا','Case Draft Documents','FATWA Admin Permissions Page',1
EXEC [dbo].pInsTranslation 'Ministry_Of_Judgement',N'وزارة العدل','Ministry of Judgement','FATWA Admin Permissions Page',1
EXEC [dbo].pInsTranslation 'Audit_Logs',N'سجل التدقيق','Audit Logs','FATWA Admin Permissions Page',1


--------------------Arshad Khan (15/01/2024)------------------------------------------------------------------------------

EXEC [dbo].pInsTranslation 'Advanced_Search',N'البحث المتقدم','Advanced Search','lmsliterature list',1

---------------------Arshad khan (18/01/2024)------------------------------------


UPDATE tTranslation SET Value_En='Government Opinion' WHERE TranslationKey='Govermemt_Opinion'

UPDATE tTranslation SET Value_En='Approve Consultation File' WHERE TranslationKey='Approve_Consultation_file'
UPDATE tTranslation SET Value_En='Approve/Reject Consultation File' WHERE TranslationKey='Approve_Reject_ConsultationFile'

EXEC [dbo].pInsTranslation 'Fatwa_Footer',N'#year# Ⓒ مجلس الوزراء الفتوى والتشريع',N'Council of Ministers Legal Advice and Legislation Ⓒ #year#','Footer Title',1

Update tTranslation set Value_Ar = N'طباعة المستند' where  TranslationKey = 'Print_Document'
------------29-jan-2024

EXEC [dbo].pInsTranslation 'Working_Time',N'وقت العمل','Working Time','Add Employee',1
EXEC [dbo].pInsTranslation 'Grade_Type',N'Grade Type','Working Time','Add Employee',1
EXEC [dbo].pInsTranslation 'Must_Six_Digits',N'يجب أن يحتوي على ستة خانات','Must Six Digits','Add Employee',1
EXEC [dbo].pInsTranslation 'Must_Twelve_Digits',N'يجب أن يحتوي على اثنا عشر خانة','Must Twelve Digits','Add Employee',1

----------------------------------------------------------Arshadkhan(06-02-24)---------------------------

EXEC [dbo].pInsTranslation 'AssignedBy',N'تمت الإحالة عن طريق','Assigned By','Time Tracking',1

EXEC [dbo].pInsTranslation 'AssignedTo',N'تم التعيين الى','Assigned To','Time Tracking',1



------01/29/2024

EXEC [dbo].pInsTranslation 'Review_Draft_Document',N'تم إرسال المسودة للمراجعة #entity#','A Draft has been sent for Review #entity#.','DocumentView',1

-----20-jan-24--

EXEC [dbo].pInsTranslation 'Employees',N'الموظفين','Employees','List Employee',1

------Ammaar Naveed-----05/02/2024---------
EXEC [dbo].pInsTranslation 'Must_Eight_Digits',N'يجب أن يحتوي على 8 احرف على الأقل','Should be minimum 8 digits','Add Employee Contact',1
EXEC [dbo].pInsTranslation 'Is_Primary',N'أساسي','Primary','Add Employee Contact',1
EXEC [dbo].pInsTranslation 'Enter_Contact_Number',N' ادخل رقم جهة الاتصال','Enter Contact Number','Add Employee Contact',1
EXEC [dbo].pInsTranslation 'Primary_Contact_Exists',N'تم اختيار رقم الاتصال الأساسي من قبل، هل تود جعل هذا الرقم اساسي؟','Primary contact already exists. Do you want to make this contact primary? (Making this contact primary will make previously added contact as non-primary).','Edit Employee Contact',1
EXEC [dbo].pInsTranslation 'Atleast_One_Primary_Contact',N'يجب اختيار رقم اساسي واحد على الأقل','There should be atleast one primary contact.','Add Employee Contact',1
EXEC [dbo].pInsTranslation 'Add_One_primary_Contact',N' يرجى اختيار رقم الاتصال الاساسي','Please add a primary contact.','Add Employee Contact',1
EXEC [dbo].pInsTranslation 'Max_Eight_Digits',N'بحد أقصى ٨ ارقام فقط','Maximum eight digits only.','Add Employee Contact',1
EXEC [dbo].pInsTranslation 'Contact_Number',N'رقم الاتصال','Contact Number','Add Employee Contact',1
EXEC [dbo].pInsTranslation 'Is_Primary',N'أساسي','Primary','Add Employee Contact',1
EXEC [dbo].pInsTranslation 'Primary',N'أساسي','Primary','Add Employee Contact',1

------Ammaar Naveed-----05/02/2024---------UMS_Claims Translations
EXEC [dbo].pInsTranslation 'Case_Consultation_Management_System',N'نظام إدارة ملفات القضايا/الاستشاري','Case/Consultation Management System','UMS_CLAIMS',1
EXEC [dbo].pInsTranslation 'Workflows',N'آليات العمل','Workflows','FATWA Admin Permissions Page',1
EXEC [dbo].pInsTranslation 'Employees',N'الموظفين','Employees','FATWA Admin Permissions Page',1
----------
EXEC [dbo].pInsTranslation 'Modified_From',N'تعديل من','Modified From','G2G Case file detail',1
EXEC [dbo].pInsTranslation 'Modified_To',N'تم تعديله إلى','Modified To','G2G Case file detail',1
EXEC [dbo].pInsTranslation 'Transfer_Consultation',N'إحالة الاستشارة','Transfer Consultation','Consultation Trasnfer Sector Popup',1
EXEC [dbo].pInsTranslation 'No_Active_Workflow',N'لا يوجد آلية عمل فعالة ، يرجى الاتصال بالمسؤول.','No active workflow detected, Please contact administrator.','Case File',1
EXEC [dbo].pInsTranslation 'ModifiedFrom_NotGreater_ModifiedTo',N'يجب أن لا يكون "تعديل من" أكبر "تعديله إلى"','Modified From should not be greater than Modified To','Lists',1
EXEC [dbo].pInsTranslation 'Government_Entity',N'الجهات الحكومية','Government Entity','Number Pattern',1
EXEC [dbo].pInsTranslation 'Case_Managment',N'ادارة القضايا','Case Management','Case Request page',1
EXEC [dbo].pInsTranslation 'Consultation_Managment',N'Consultation Management','Consultation Management','Consultation Managment',1
---------

-----------------------------02/05/2024
EXEC [dbo].pInsTranslation 'Time_Log',N'سجل الوقت','Time Log','Time Log',1
EXEC [dbo].pInsTranslation 'Status',N'حالة','Status','Common',1
EXEC [dbo].pInsTranslation 'Transfer_Request_Initiated',N'تم إحالة الطلب بنجاح','Transfer request has been initiaded successfully.','Common',1
EXEC [dbo].pInsTranslation 'AdministrativeRegionalCases',N'قطاع الكلي الاداري','Administrative Regional Cases','AssignLawyertoCourt',1
EXEC [dbo].pInsTranslation 'AdministrativeSupremeCases',N'قطاع التمييز الاداري','Administrative Supreme Cases','AssignLawyertoCourt',1
EXEC [dbo].pInsTranslation 'AdministrativeAppealCases',N'قطاع الاستئناف الاداري','Administrative Appeal Cases','AssignLawyertoCourt',1
EXEC [dbo].pInsTranslation 'Select_Sector',N'اختر قطاع','Select Sector','AssignToLawyer',1
EXEC [dbo].pInsTranslation 'CivilCommercialRegionalCases',N'قطاع الكلي المدني التجاري','Civil Commercial Regional Cases','AssignLawyertoCourt',1
EXEC [dbo].pInsTranslation 'CivilCommercialAppealCases',N'قطاع الاستئناف المدني التجاري','Civil Commercial Appeal Cases','AssignLawyertoCourt',1
EXEC [dbo].pInsTranslation 'CivilCommercialSupremeCases',N'قطاع التمييز المدني التجاري ','Civil Commercial Supreme Cases','AssignLawyertoCourt',1
EXEC [dbo].pInsTranslation 'Review_Draft_Document_Task',N'مراجعة مسودة المستند','Review Draft Document','Task List',1
EXEC [dbo].pInsTranslation 'Add_Assign_HOS_Consultation_File_Success',N'تم تعيين ملف استشاري جديد #entity# بنجاح، يرجى زيارة الرابط المرفق للمزيد من التفاصيل','New consultation file #entity# has been assigned successfully, please visit the provided link for more detailsNew consultation file #entity# has been assigned successfully, please visit the provided link for more details','Notification',1
EXEC [dbo].pInsTranslation 'Add_Assign_HOS_Consultation_Request_Success',N'تم تعيين طلب استشاري جديد #entity# بنجاح، يرجى زيارة الرابط المرفق للمزيد من التفاصيل','New Consultation Request #entity#  Has been Assign Successfully.Please Visit Provided link for More details','Notification',1
EXEC [dbo].pInsTranslation 'Decision_Pending_Of_Attendee',N'دعوة لحضور اجتماع جديدة بانتظار قرارك، يرجى الضغط على الرابط المرفق للمزيد من التفاصيل #entity#','A new meeting invite is pending for your decision, please visit the provided link for more details #entity#','Notification',1
EXEC [dbo].pInsTranslation 'Draft_Modification',N'تم إرسال المسودة للتعديل #entity#','A Draft has been sent for Modification #entity#.','Notification',1
EXEC [dbo].pInsTranslation 'Edit_Case_Success',N'# entity# يتم تحديثه بنجاح في النظام. يرجى زيارة الرابط المرفق لمزيد من التفاصيل.','#entity# is successfully updated in the system. Please visit provided link for more details.','Notification',1
EXEC [dbo].pInsTranslation 'MOJ_Recived_Registerd_Request_Success',N'تم استلام الطلب المسجل #entity# بنجاح، يرجى زيارة الرابط للمزيد من التفاصيل','New Registered Request #entity# has been received successfully.Please visit provided link for more details','Notification',1
EXEC [dbo].pInsTranslation 'Registered_Case_Notification',N'تم تسجيل طلب القضية بنجاح #entity#','Case Request has been registered successfully.#entity#','Notification',1
EXEC [dbo].pInsTranslation 'Add_Hearing',N'إضافة الجلسة','Add Hearing','Notification',1
EXEC [dbo].pInsTranslation 'Add_Judgement',N'إضافة الحكم','Add Judgement','Notification',1
EXEC [dbo].pInsTranslation 'Add_Outcome',N'إضافة الحصيلة','Add Outcome','Notification',1
EXEC [dbo].pInsTranslation 'Case_Merge_Request_Task',N'تم ضم القضايا','Case Merge Request Task','Draft Detail',1
EXEC [dbo].pInsTranslation 'Postpone_Hearing',N'تأجيل جلسة المحكمة','Postpone Hearing','Case Management',1
EXEC [dbo].pInsTranslation 'Review_DMS_Document',N'مراجعة مستند لنظام ادارة المستندات','Review DMS Document','Workflow',1
EXEC [dbo].pInsTranslation 'Review_Recieved_Copy_of_Case_Request',N'مراجعة نسخة طلب رفع قضية','Review Recieved Copy of Case Request','Draft Detail',1
EXEC [dbo].pInsTranslation 'Save_Meeting_Task',N'تم حفظ الاجتماع','Save Meeting Task','Task List',1
EXEC [dbo].pInsTranslation 'Transfer_of_Sector_Task',N'تم تحويل الطلب','Transfer of Sector Task','Task List',1
EXEC [dbo].pInsTranslation 'Options',N'الخيارات','Options','Create Workflow',1
EXEC [dbo].pInsTranslation 'Request_For_More_Information',N'طلب المزيد من المعلومات','Request For More Information','Draft Detail',1
EXEC [dbo].pInsTranslation 'Review_Minutes_of_Meeting_Task',N'مراجعة محضر الاجتماع','Review Meeting Minutes','Meeting',1
EXEC [dbo].pInsTranslation 'Save_MOM_Meeting_Task',N'حفظ محضر الاجتماع','Save Meeting Minutes','Meeting',1
EXEC [dbo].pInsTranslation 'Additiona_GE_Users',N'مستخدمي جهات حكومية إضافيين','Additional GE Users','Task List',1
EXEC [dbo].pInsTranslation 'Withdraw_Request_Accepted',N'تمت الموافقة على سحب الطلب','Withdraw Request Accepted','CMS',1
EXEC [dbo].pInsTranslation 'Transfer_Rejection_of_Sector_Task',N'تم رفض تحويل القطاع','Sector Transfer Rejected','Task Screen',1
EXEC [dbo].pInsTranslation 'Duration_Days',N'المدة (أيام)','Duration (Days)','Task Screen',1
EXEC [dbo].pInsTranslation 'CompleteOn',N'اكتمل بتاريخ','Completed On','Task Screen',1
EXEC [dbo].pInsTranslation 'AssignedOn',N'تم تعيينه بتاريخ','Assigned On','Time Log',1
EXEC [dbo].pInsTranslation 'Execution_File_Details_Added_By_MOJ',N'تم إضافة تفاصيل ملف التنفيذ من قبل مندوب الاطلاع ','Execution file details have been added by MOJ messenger','MOJ',1
EXEC [dbo].pInsTranslation 'Document_Submitted_For_Review',N'تم ارسال المستند للمراجعة ','Document has been submitted for review','DMS',1
EXEC [dbo].pInsTranslation 'Document_Portfolio_Request_Created',N'تم إنشاء طلب حافظة مستندات ','Document Portfolio Request has been created','view Task Page',1
EXEC [dbo].pInsTranslation 'ActivityName',N'اسم النشاط ','Activity Name','Time Log',1
EXEC [dbo].pInsTranslation 'No_GE_Reply',N'لم يتم الرد من قبل الجهة الحكومية، #entity# ','No response from GE, #entity#','Communication',1
EXEC [dbo].pInsTranslation 'Review_Coms_Draft_Document_Edit',N'تم تعديل المسودة وارسالها للمراجعة،  #entity#','Draft Document has been edited and sent for review, #entity#','User Groups page',1
EXEC [dbo].pInsTranslation 'Need_More_Info_Request_Success',N'تم إنشاء طلب اخطار، #entity# ','Legal Notification request has been created, #entity# ','Notification Module',1
EXEC [dbo].pInsTranslation 'Moj_Case_Draft_Registration',N'تم ارسال المستند لتسجيل القضية، #entity#','Document has been sent for case registration, #entity#','CreateRegisteredCase',1
EXEC [dbo].pInsTranslation 'Interpretation_Of_Judgment_Added',N'قامت الجهة الحكومية بإضافة رأيها بشأن التنفيذ وذلك بتفسير الحكم المرسل، #entity#','GE has added their decision on the execution demanding more clarification on this judgment, #entity#','G2G',1
EXEC [dbo].pInsTranslation 'DMS_Document_Modification',N'تم ارسال مسودة المستند للتعديل عليها، #entity#','Draft Document has been sent for modification, #entity#','DocumentView',1
EXEC [dbo].pInsTranslation 'Option_Select_Confirm',N'هل أنت متأكد أنك تود اختيار هذا الخيار؟','Are you sure you want to select this option?','Select Option Popup',1
EXEC [dbo].pInsTranslation 'Consultation_Requests_Review',N'مراجعة طلبات الاستشاري','Consultation Requests Review','Common',1
EXEC [dbo].pInsTranslation 'Max_Eight_Digits',N'بحد أقصى ٨ ارقام فقط','Maximum eight digits only.','Add Employee Contact',1
EXEC [dbo].pInsTranslation 'Enter_Contact_Number',N'ادخل رقم جهة الاتصال','Enter Contact Number','Add Employee Contact',1
EXEC [dbo].pInsTranslation 'Primary_Contact_Exists',N'تم اختيار رقم الاتصال الأساسي من قبل، هل تود جعل هذا الرقم اساسي؟','Primary contact already exists. Do you want to make this contact primary? (Making this contact primary will make previously added contact as non-primary).','Edit Employee Contact',1
EXEC [dbo].pInsTranslation 'Atleast_One_Primary_Contact',N'يجب اختيار رقم اساسي واحد على الأقل','There should be atleast one primary contact.','Add Employee Contact',1
EXEC [dbo].pInsTranslation 'Add_One_primary_Contact',N' يرجى اختيار رقم الاتصال الاساسي','Please add a primary contact.','Add Employee Contact',1

										------------------PACI Address Query---------------
EXEC [dbo].pInsTranslation 'Select_One_Of_these',N'اختر نوع الطلب','Select Request Type','PACI Request List',1
EXEC [dbo].pInsTranslation 'Under_Filing',N'تحت الرفع','Under Filing','PACI Request List',1
EXEC [dbo].pInsTranslation 'Year',N'السنة','Year','PACI Request List',1
EXEC [dbo].pInsTranslation 'Address_Type',N'نوع العنوان','Address Type','PACI Request List',1
EXEC [dbo].pInsTranslation 'Identity_Information',N'معلومات الهوية','Identity Information','PACI Request List',1
EXEC [dbo].pInsTranslation 'Civil_ID',N'الرقم المدني','Civil Id','PACI Request List',1
EXEC [dbo].pInsTranslation 'New_Requests',N'طلبات جديدة','New Request','PACI Request List',1
EXEC [dbo].pInsTranslation 'Submitted_Requests',N'الطلبات المرسلة','Submitted Requests','PACI Request List',1
EXEC [dbo].pInsTranslation 'Residential_Address',N'عنوان السكن','Residential Address','PACI Request List',1
EXEC [dbo].pInsTranslation 'Work_Address',N'عنوان العمل','Work Address','PACI Request List',1
EXEC [dbo].pInsTranslation 'Email_Sent',N'تم إرسال النموذج عن طريق البريد الإلكتروني','Email Sent','PACI Request List',1
EXEC [dbo].pInsTranslation 'Extracted_Data',N'البيانات المستخرجة','Extracted Data','PACI Request List',1
EXEC [dbo].pInsTranslation 'Request_Form',N'نموذج الطلب','Request Form','PACI Request List',1
EXEC [dbo].pInsTranslation 'Response_Document',N'المستندات المرسلة','Response Document','PACI Request List',1
EXEC [dbo].pInsTranslation 'Refrence_Number',N'Refrence Number','Refrence Number','PACI Request List',1
EXEC [dbo].pInsTranslation 'Civil_Must_have_12',N'رقم البطاقة المدنية يجب أن يكون ١٢ رقم','Civil id must have 12 digit','PACI Request List',1
EXEC [dbo].pInsTranslation 'Fill_At_Least_Name',N'يجب إدخال الاسم','Fill At Least Name','PACI Request List',1
EXEC [dbo].pInsTranslation 'Select_Address_Type',N'اختر نوع العنوان','Select Address Type','PACI Request List',1
EXEC [dbo].pInsTranslation 'Request_Confarmation_Massage',N'هل أنت متأكد أنك تريد حذف هذا الطلب؟
ستتم إزالة جميع التفاصيل المقدمة مع هذا الطلب.','Are you sure you want to delete this request?
All the details submitted with this request will be removed.','PACI Request List',1
EXEC [dbo].pInsTranslation 'Request_Deleted',N'تم حذف الطلب','Request Deleted','PACI Request List',1
EXEC [dbo].pInsTranslation 'Fill_Required_Fields',N'تم حذف الطلب','Request Deleted','PACI Request List',1
EXEC [dbo].pInsTranslation 'Fill_Identification_Grid_First',N'Fill Identification Grid First','Fill Identification Grid First','PACI Request List',1
EXEC [dbo].pInsTranslation 'Request_Success_Massage',N'طلب إرسال بنجاح',' Request Submit Successfully','PACI Request List',1
EXEC [dbo].pInsTranslation 'Conformation_RequestSubmit_Massage',N'هل أنت متأكد أنك تريد إرسال هذا الطلب',' Are you sure you want to submit this request','PACI Request List',1
EXEC [dbo].pInsTranslation 'Row_has_been_Deleted',N'Row has been Deleted','Row has been Deleted','PACI Request List',1
EXEC [dbo].pInsTranslation 'Requested_Details',N'التفاصيل المطلوبة','Requested Details','PACI Request List',1
EXEC [dbo].pInsTranslation 'Residency_Number',N'طلب إرسال بنجاح','Residency Number','PACI Request List',1
EXEC [dbo].pInsTranslation 'Birth_Date',N'تاريخ الميلاد','Birth Date','PACI Request List',1
EXEC [dbo].pInsTranslation 'Avenue',N'جادة','Avenue','PACI Request List',1
EXEC [dbo].pInsTranslation 'Block',N'القطعة','Block','PACI Request List',1
EXEC [dbo].pInsTranslation 'Street_Name',N'اسم الشارع','Street Name','PACI Request List',1
EXEC [dbo].pInsTranslation 'Floor',N'الطابق','Floor','PACI Request List',1
EXEC [dbo].pInsTranslation 'Housing_Type',N'نوع السكن','Housing Type','PACI Request List',1
EXEC [dbo].pInsTranslation 'Housing_Number',N'رقم السكن','Housing Number','PACI Request List',1
EXEC [dbo].pInsTranslation 'Address_Automated_Number',N'الرقم الآلي للعنوان','Address Automated Number','PACI Request List',1
EXEC [dbo].pInsTranslation 'Branched_From',N'متفرعة من','Branched From','PACI Request List',1
EXEC [dbo].pInsTranslation 'Building_Name',N'اسم المبنى','Building Name','PACI Request List',1
EXEC [dbo].pInsTranslation 'Other_Data',N'بيانات أخرى','Other Data','PACI Request List',1
EXEC [dbo].pInsTranslation 'Phone_Number',N'رقم الهاتف','Phone Number','PACI Request List',1
EXEC [dbo].pInsTranslation 'Paci_Request_List',N'Paci_Request_List','PACI Request List','PACI Request List',1
EXEC [dbo].pInsTranslation 'Must_nine_Characters',N'Case Number Must be 9 digits','Case Number Must be 9 digits','PACI Request List',1
--ODRP--
EXEC [dbo].pInsTranslation 'On_Demand_Requests',N'طلب معلومات من الجهات الحكومية','On Demand Requests','ODRP',1
EXEC [dbo].pInsTranslation 'On_Demand_Request_Portal',N'نظام طلب معلومات من الجهات الحكومية','On Demand Request Portal','ODRP',1
EXEC [dbo].pInsTranslation 'MOJ_Rolls',N'نظام طلب رول من وزارة العدل','MOJ Rolls','ODRP',1
EXEC [dbo].pInsTranslation 'PACI_Address_Query',N'نظام طلب عنوان من الهيئة العامة للمعلومات المدنية','PACI Address Query','ODRP',1
EXEC [dbo].pInsTranslation 'Moj_Statistics',N'إحصائيات وزارة العدل','Moj Statistics','ODRP',1
EXEC [dbo].pInsTranslation 'Moj_Statistics_Case_Study',N'إحصائيات وزارة العدل (دراسة القضية)','MOJ Statistics (Case Study)','ODRP',1
EXEC [dbo].pInsTranslation 'Kuwait_AlYawm',N'الكويت اليوم','Kuwait Al-Yawm','ODRP',1

---------MOJ ROlls--------

EXEC [dbo].pInsTranslation 'Please_Select_Atleast_One_Attendee',N'يرجى اختيار أحد الحاضرين على الأقل','Please Select Atleast One Attendee','meeting add',1
EXEC [dbo].pInsTranslation 'try_again',N'إعادة المحاولة','try again','MOJ Rolls Execption view  Page',1
EXEC [dbo].pInsTranslation 'Exception_details',N' تفاصيل الاستثناء','Exception details','MOJ Rolls Execption view  Page',1
EXEC [dbo].pInsTranslation 'MOJ_New_Request',N' إنشاء طلب جديد','New Request','MOJ Rolls List view  Page',1

--------MOJ Statistic Dashboard-------------
EXEC [dbo].pInsTranslation 'Case_Registered_In_Favor_Of_GE',N' إنشاء طلب جديد','Case Registered In Favor of GE','Moj Statisctics Dashboard',1
EXEC [dbo].pInsTranslation 'Against_GE',N' ضد الدولة','Against GE','Moj Statisctics Dashboard',1
EXEC [dbo].pInsTranslation 'CaseAutomated_Number',N' الرقم الآلي للقضايا','Case Automated Number','Moj Statisctics Dashboard',1
EXEC [dbo].pInsTranslation 'Judgements_Detail',N' تفاصيل الأحكام','Judgements Details','Moj Statisctics Dashboard',1
EXEC [dbo].pInsTranslation 'Document_Number',N' رقم الوثيقة','Document Number','Moj Statisctics Dashboard',1
EXEC [dbo].pInsTranslation 'Judgments_Issued',N' الأحكام الصادرة','Judgments Issued','Moj Statisctics Dashboard',1
EXEC [dbo].pInsTranslation 'New_Case_Registered',N'القضايا المسجلة','Registered Cases','Moj Statisctics Dashboard',1
EXEC [dbo].pInsTranslation 'Case_Registered_In_Favor_Of_GE',N'قضايا مسجلة لصالح الدولة','Case Registered In Favor of GE','Moj Statisctics Dashboard',1
EXEC [dbo].pInsTranslation 'Case_Registered_Against_GE',N'قضايا مسجلة ضد الدولة','Case Registered Against GE','Moj Statisctics Dashboard',1
EXEC [dbo].pInsTranslation 'Total_Case',N'عدد القضايا','Total Cases','Moj Statisctics Dashboard',1
EXEC [dbo].pInsTranslation 'Judgement_Statement',N'منطوق الحكم','Judgements Statements','Moj Statisctics Dashboard',1
EXEC [dbo].pInsTranslation 'CaseAutomatedNumber',N'رقم الحالة الآلي','CaseAutomatedNumber','Case Study',1
EXEC [dbo].pInsTranslation 'In_Favor_Of_GE',N'لصالح الدولة','In Favor of GE','Moj Statistic',1
EXEC [dbo].pInsTranslation 'Custome_Rolls',N'طلبات الرول المخصصة','MOJ Custome Rolls','Moj Custom Rolls List',1

EXEC [dbo].pInsTranslation 'Case_Migrated_From_MOJ_Notification',N'تم تسجيل قضية جديد في وزارة العدل، #entity#','New case registered in MOJ. Please Visit Provided link for More details. #entity#','MOJ RPA',1
EXEC [dbo].pInsTranslation 'Case_Migrated_From_MOJ',N'تم تسجيل قضية جديد في وزارة العدل','New case registered in MOJ','MOJ RPA',1

------19-feb-2024------
EXEC [dbo].pInsTranslation 'Users_Without_Group',N'مستخدمون بدون مجموعة','Users Without Groups','UMS Group',1
EXEC [dbo].pInsTranslation 'Users_With_Group',N'مستخدمون مع المجموعة','Users With Groups','UMS Group',1

---Ammaar Naveed---27/02/2024----
UPDATE tTranslation SET Value_En='Please fill the required fields.' WHERE TranslationKey='Fill_Required_Fields'


------------------------------------------------------Arshad khan (21-02-2024)-----------------------------------------------------

EXEC [dbo].pInsTranslation 'Create_Final_Draft',N'Create Final Draft','Create Final Draft','Legal Notifications',1

EXEC [dbo].pInsTranslation 'AssignedToDepartmentName',N'تم تعيينه للقسم','Assigned To Department','View Case File Details',1

EXEC [dbo].pInsTranslation 'AssignedByDepartmentName',N'تم تعيينه من قبل القسم','Assigned By Department','View Case File Details',1

EXEC [dbo].pInsTranslation 'Execution_Request_Details',N'تفاصيل طلب التنفيذ','Execution Request Details','View Details',1

---Ammaar Naveed----21/02/2024---Side Menu Translations
EXEC [dbo].pInsTranslation 'Users_Management',N'نظام إدارة المستخدمين','Users Management','Side Menu',1
EXEC [dbo].pInsTranslation 'UMS_Configuration',N'إعدادات نظام إدارة المستخدمين','UMS Configuration','Side Menu',1
EXEC [dbo].pInsTranslation 'System_Settings',N'إعدادات النظام','System Settings','Side Menu',1
EXEC [dbo].pInsTranslation 'Library_Configuration',N'إعدادات نظام إدارة المكتبات','Library Configuration','Side Menu',1
EXEC [dbo].pInsTranslation 'Case_Configuration',N'إعدادات نظام القضايا','Case Configuration','Side Menu',1
EXEC [dbo].pInsTranslation 'Consultation_Configuration',N'إعدادات نظام الاستشاري','Consultation Configuration','Side Menu',1
EXEC [dbo].pInsTranslation 'DMS_Configuration',N'إعدادات نظام إدارة المستندات','DMS Configuration','Side Menu',1
EXEC [dbo].pInsTranslation 'Common_Configuration',N'الإعدادات العامة','Common Configuration','Side Menu',1
EXEC [dbo].pInsTranslation 'Background_Services',N'خدمات النظام','Background Services','Side Menu',1
EXEC [dbo].pInsTranslation 'Translations',N'الترجمات','Translations','Side Menu',1
EXEC [dbo].pInsTranslation 'Communication',N'التواصل','Communication','General',1

---Ammaar Naveed----29/02/2024---Sidemenu and other translations-----------------
UPDATE tTranslation SET Value_En='FATWA ADMINISTRATION PORTAL' WHERE TranslationKey='FATWA_ADMINISTRATION_PORTAL'
UPDATE tTranslation SET VAlue_Ar=N'بوابة إدارة نظام الفتوى' WHERE TranslationKey='FATWA_ADMINISTRATION_PORTAL'
EXEC [dbo].pInsTranslation 'Group_Access_Types',N'أنواع مجموعة الصلاحيات','Group Access Types','Side Menu',1
EXEC [dbo].pInsTranslation 'Claims',N'المطالبات','Claims','Side Menu',1
EXEC [dbo].pInsTranslation 'Assign_Supervisor',N'تعيين المشرف','Assign Supervisor','Side Menu',1
EXEC [dbo].pInsTranslation 'Departments',N'الأقسام','Departments','Side Menu',1
EXEC [dbo].pInsTranslation 'Legal_Library',N'المكتبة القانونية','Legal Library','Side Menu',1
EXEC [dbo].pInsTranslation 'Legislation_Configuration',N'إعدادات التشريعات','Legislation Configuration','Side Menu',1
EXEC [dbo].pInsTranslation 'Legal_Principle_Configuration',N'إعدادات المبدأ القانوني','Legal Principle Configuration','Side Menu',1
EXEC [dbo].pInsTranslation 'Dynamic_Lookups',N'القوائم الديناميكية','Dynamic Lookups','Side Menu',1
EXEC [dbo].pInsTranslation 'Fixed_Lookups',N'قوائم ثابتة','Fixed Lookups','Side Menu',1
EXEC [dbo].pInsTranslation 'Case_Request_Number_Pattern',N'نمط رقم طلب القضية','Case Request Number Pattern','Side Menu',1
EXEC [dbo].pInsTranslation 'Case_File_Number_Pattern',N'نمط رقم ملف القضية','Case File Number Pattern','Side Menu',1
EXEC [dbo].pInsTranslation 'Consultation_Request_Number_Pattern',N'نمط رقم الطلب الاستشاري','Consultation Request Number Pattern','Side Menu',1
EXEC [dbo].pInsTranslation 'Consultation_File_Number_Pattern',N'نمط رقم الملف الاستشاري','Consultation File Number Pattern','Side Menu',1
EXEC [dbo].pInsTranslation 'DMS_Fixed_Lookups',N'قوائم البحث الثابتة DMS','DMS Fixed Lookups','Side Menu',1
EXEC [dbo].pInsTranslation 'Inbox_Number',N'رقم الوارد','Inbox Number','Side Menu',1
EXEC [dbo].pInsTranslation 'Outbox_Number',N'رقم الصادر','Outbox Number','Side Menu',1
EXEC [dbo].pInsTranslation 'Common_Lookups',N'قوائم شائعة الاستخدام','Common Lookups','Side Menu',1
EXEC [dbo].pInsTranslation 'SLA_Intervals',N'فترات SLA','SLA Intervals','Side Menu',1
EXEC [dbo].pInsTranslation 'Execution_Logs',N'سجل التنفيذ','Execution Logs','Side Menu',1
EXEC [dbo].pInsTranslation 'Public_Holidays',N'إجازات رسمية','Public Holidays','Side Menu',1
--------------29/02/2024
EXEC [dbo].pInsTranslation 'Time_Log_List_Contract_File',N'سجل الوقت ملفات العقود','Time Log Contract Files','Time Log',1
EXEC [dbo].pInsTranslation 'Time_Log_List_Legal_Advice_File',N'سجل الوقت ملفات الفتوى','Time Log Legal Advice Files','Time Log',1
EXEC [dbo].pInsTranslation 'Time_Log_List_International_Arbitration_File',N'سجل الوقت التحكيم الدولي','Time Log International Arbiteration Files','Time Log',1
EXEC [dbo].pInsTranslation 'Time_Log_Administrative_Complaints_File',N'سجل الوقت  التظلمات الإدارية','Time Log Administrative Complaint Files','Time Log',1
EXEC [dbo].pInsTranslation 'Time_Log_List_Legislations_File',N'سجل الوقت ملفات التشريع','Time Log Legislation Files','Time Log',1
EXEC [dbo].pInsTranslation 'Time_Log_Consultation_Request_List',N'سجل الوقت قائمة طلب الاستشاري','Time Log Consultation Request List','Time Log',1
EXEC [dbo].pInsTranslation 'Time_Log_Contracts_Request_List',N'سجل الوقت طلب العقود','Time Log Contract Request','Time Log',1
EXEC [dbo].pInsTranslation 'Time_Log_International_Arbitration_Request_List',N'سجل الوقت طلب التحكيم الدولي','Time Log International Arbiteration Request','Time Log',1
EXEC [dbo].pInsTranslation 'Time_Log_Legal_Advice_Request_List',N'سجل الوقت طلب الفتوى','Time Log Legal Advice Request','Time Log',1
EXEC [dbo].pInsTranslation 'Time_Log_Legislations_Request_List',N'سجل الوقت طلب تشريع','Time Log Legislation Request','Time Log',1
EXEC [dbo].pInsTranslation 'Time_Log_Administrative_Complaints_Request_List',N'سجل الوقت طلب التظلمات الإدارية','Time Log Administrative Complaint Request','Time Log',1
EXEC [dbo].pInsTranslation 'Time_Log_Requests_List',N'سجل الوقت قائمة الطلبات','Time Log Requests List','Time Log',1
EXEC [dbo].pInsTranslation 'TimeLog_Request_List',N'سجل الوقت قائمة الطلبات','Time Log Requests List','Time Log',1
---Case Time Log   
EXEC [dbo].pInsTranslation 'Time_Log_Under_Filing',N'سجل الوقت تحت الرفع','Time Log Under Filing','Time Log',1
EXEC [dbo].pInsTranslation 'Time_Log_Case_Requests',N'سجل الوقت طلبات القضية','Time Log Case Requests','Time Log',1
EXEC [dbo].pInsTranslation 'Time_Log_Case_Files',N'سجل الوقت ملفات القضية','Time Log Case Files','Time Log',1
---Ammaar Naveed----03/03/2024------
UPDATE tTranslation SET Value_En='Username' WHERE TranslationKey='UserName'
UPDATE tTranslation SET TranslationKey='Deactivate_Confirm_Message' WHERE TranslationKey='deactivate_confirm_message'
EXEC [dbo].pInsTranslation 'Execution_Request_Created_For_Review_Notification',N'تم ارسال طلب التنفيذ، #entity#','Execution request has been submitted, #entity#','CMS',1
-------------------------------------------------------arshad khan (5/3/24)-----------------------------
EXEC [dbo].pInsTranslation 'CompleteDate',N'تاريخ الانجاز','CompleteDate','Case File',1
EXEC [dbo].pInsTranslation 'Sure_Save_TaskResponse',N'هل أنت متأكد أنك تريد حفظ الرد على المهمة؟','Are you sure you want to save the Task Response?','Task Response Detail View Page',1
EXEC [dbo].pInsTranslation 'TaskResponse_Save_Success',N'تم حفظ الرد على المهمة بنجاح','Task Response Saved Successfully','Task Response Detail View Page',1
---Ammaar Naveed----07/03/2024------
EXEC [dbo].pInsTranslation 'Sector_Type_History',N'التغيرات نوع القطاع','Sector Type History','Sector Type Lookup History',1
---Ammaar Naveed----12/03/2024------
EXEC [dbo].pInsTranslation 'Case',N'القضية','Case','Permissions Grid',1
EXEC [dbo].pInsTranslation 'Employee',N'إدارة شؤون الموظفين','Employee','Permissions Grid',1
EXEC [dbo].pInsTranslation 'CMS_COMS',N'القضية/الاستشارة','CMS/COMS','Permissions Grid',1
EXEC [dbo].pInsTranslation 'Consultation',N'الاستشارة','Consultation','Permissions Grid',1

-----------------------------------------
EXEC [dbo].pInsTranslation 'Reciever_GE',N'Reciever GE','Reciever GE','List More Information',1
EXEC [dbo].pInsTranslation 'Entity_Action_History',N'Entity Action History','Entity Action History','Item View History',1
EXEC [dbo].pInsTranslation 'Entity_Action',N'Action','Action','Item View History',1
EXEC [dbo].pInsTranslation 'View_Item_History',N'View Item History','View Item History','Item View History',1
EXEC [dbo].pInsTranslation 'Case_Request_History',N'Case Request History','Case Request History','Item View History',1
EXEC [dbo].pInsTranslation 'Case_File_History',N'Case File History','Case File History','Item View History',1
EXEC [dbo].pInsTranslation 'Registered_Case_History',N'Registered Case History','Registered Case History','Item View History',1
EXEC [dbo].pInsTranslation 'Coms_File_History',N'Consultation File History','Consultation File History','Item View History',1
EXEC [dbo].pInsTranslation 'Coms_Request_History',N'Consultation Request History','Consultation Request History','Item View History',1
EXEC [dbo].pInsTranslation 'Item_History',N'History','History','Item View History',1
EXEC [dbo].pInsTranslation 'Edit_Outcome',N'Edit Outcome','Edit Outcome','Registered Case Detail',1


EXEC [dbo].pInsTranslation 'TaskResponse_Save_Success',N'تم حفظ الرد على المهمة بنجاح','Task Response Saved Successfully','Task Response Detail View Page',1
EXEC [dbo].pInsTranslation 'Reason_Section',N'سبب الإعادة','Reason of returning','Draft Detail',1
EXEC [dbo].pInsTranslation 'Return_To_Lawyer',N'إعادة للعضو','Return To Lawyer','Draft Detail',1
EXEC [dbo].pInsTranslation 'Version_History',N'تغيرات الإصدارات','Version History','Document',1

EXEC [dbo].pInsTranslation 'Arial',N'Arial','Arial','Radzen Html Editor',1
EXEC [dbo].pInsTranslation 'Georgia',N'Georgia','Georgia','Radzen Html Editor',1
EXEC [dbo].pInsTranslation 'Helvetica',N'Helvetica','Helvetica','Radzen Html Editor',1
EXEC [dbo].pInsTranslation 'Monospace',N'Monospace','Monospace','Radzen Html Editor',1
EXEC [dbo].pInsTranslation 'Segoe_UI',N'Segoe UI','Segoe UI','Radzen Html Editor',1
EXEC [dbo].pInsTranslation 'Tahoma',N'Tahoma','Tahoma','Radzen Html Editor',1
EXEC [dbo].pInsTranslation 'Times_New_Roman',N'Times New Roman','Times New Roman','Radzen Html Editor',1
EXEC [dbo].pInsTranslation 'Verdana',N'Verdana','Verdana','Radzen Html Editor',1

EXEC [dbo].pInsTranslation 'Normal_Text',N'Normal','Normal','Radzen Html Editor',1
EXEC [dbo].pInsTranslation 'Heading_1',N'Heading 1','Heading 1','Radzen Html Editor',1
EXEC [dbo].pInsTranslation 'Heading_2',N'Heading 2','Heading 2','Radzen Html Editor',1
EXEC [dbo].pInsTranslation 'Heading_3',N'Heading 3','Heading 3','Radzen Html Editor',1
EXEC [dbo].pInsTranslation 'Heading_4',N'Heading 4','Heading 4','Radzen Html Editor',1
EXEC [dbo].pInsTranslation 'Heading_5',N'Heading 5','Heading 5','Radzen Html Editor',1
EXEC [dbo].pInsTranslation 'Heading_6',N'Heading 6','Heading 6','Radzen Html Editor',1

EXEC [dbo].pInsTranslation 'Font_Size',N'Font Size','Font Size','Radzen Html Editor',1
EXEC [dbo].pInsTranslation 'Format_Block',N'Format Block','Format Block','Radzen Html Editor',1
EXEC [dbo].pInsTranslation 'Font',N'Font','Font','Radzen Html Editor',1
--Ammaar Naveed--13/03/2024--
EXEC [dbo].pInsTranslation 'Add_UMS_Group',N'Are you sure you want to add this group?','Are you sure you want to add this group?','Add Group',1
EXEC [dbo].pInsTranslation 'Update_UMS_Group',N'Are you sure you want to update this group?','Are you sure you want to update this group?','Add Group',1
--Ammaar Naveed--21/03/2024--
EXEC [dbo].pInsTranslation 'Success',N'تمت العملية بنجاح','Success','Employee Activity View',1


-----Hassan 27/03/24

EXEC [dbo].pInsTranslation 'Update_Notification_Event',N'تحديث الإشعا','Update Notification Event','Notification',1
EXEC [dbo].pInsTranslation 'Body_En',N'المحتوى (انجليزي','Body English','Notification',1
EXEC [dbo].pInsTranslation 'Body_Ar',N'المحتوى(عربي)','Body Arabic','Notification',1
EXEC [dbo].pInsTranslation 'Channel',N'قناة الإشعار','Channel','Notification',1
EXEC [dbo].pInsTranslation 'Event_List',N'قائمة الإشعارات','Event List','Notification',1
EXEC [dbo].pInsTranslation 'Create_Notification_Event',N'إنشاء إشعار','Create Notification Event','Notification',1
EXEC [dbo].pInsTranslation 'Create_Template_Event',N'إنشاء قالب للإشعا','Create Template Event','Notification',1

EXEC [dbo].pInsTranslation 'Reciever_Type',N'نوع المستلم','Reciever type','Notification',1





EXEC [dbo].pInsTranslation 'Courts',N'المحاكم','Courts','CMS',1 
EXEC [dbo].pInsTranslation 'Chambers',N'الدوائر','Chambers','CMS',1 
EXEC [dbo].pInsTranslation 'Chambers',N'الدوائر','Chambers','CMS',1 

/*<History Author='Umer Zaman' Date='04-04-2024'> Script start </History>*/

EXEC [dbo].pInsTranslation 'ISBN',N'الرقم العالمي الموحد للكتاب (20)','ISBN Number (20)','Add Lms Literature page',1
EXEC [dbo].pInsTranslation 'Name_Tag_Number',N'الاسم (1212)','Name (1212)','Add Lms Literature page',1
EXEC [dbo].pInsTranslation 'Due_Date',N'يجب اعادته قبل','Due Date','Case File',1
EXEC [dbo].pInsTranslation 'Reserved',N'مستعار','Reserved','Lms Literature List Page',1
EXEC [dbo].pInsTranslation 'The_Principle',N'نص المبدأ','Principle Content','Legal Principle Preview Page ',1
EXEC [dbo].pInsTranslation 'Due_Date',N'يجب اعادته قبل','Due Date','Case File',1
EXEC [dbo].pInsTranslation 'Principle_Issue_Date',N'تاريخ الانشاء','Created Date','Legal Principle Preview Page ',1
EXEC [dbo].pInsTranslation 'Principle_Issue_Date_Hijri',N'تاريخ الانشاءالهجري','Created Date Hijri','Legal Principle Preview Page ',1
EXEC [dbo].pInsTranslation 'Due_Date_Borrow',N'يجب اعادته قبل','Must Returned Before','Legal Principle Preview Page ',1
/*<History Author='Umer Zaman' Date='04-04-2024'> Script end </History>*/


------------- 16-4-2024 ---------------
EXEC [dbo].pInsTranslation 'Browser',N'صفحة الويب','Browser','Event List',1
EXEC [dbo].pInsTranslation 'Mobile',N'الهاتف النقال','Mobile','Event List',1
EXEC [dbo].pInsTranslation 'Templates',N'Templates','Templates','Event List',1
EXEC [dbo].pInsTranslation 'Event_Name',N'Event Name','Event Name','Event List Pop',1
EXEC [dbo].pInsTranslation 'Sure_Want_To_Activate_Event',N'هل أنت متاكد أنك تريد تنشيط الحدث؟','Are you sure you want to activate the event?','Event List',1
EXEC [dbo].pInsTranslation 'Sure_Want_To_Deactivate_Event',N'هل أنت متأكد من أنك تريد تعطيل الحدث؟','Are you sure you want to deactivate event?','Event List',1
EXEC [dbo].pInsTranslation 'Sure_Want_To_Activate_Template',N'هل أنت متاكد أنك تريد تنشيط القالب؟','Are you sure you want to activate the template?','Event List',1
EXEC [dbo].pInsTranslation 'Sure_Want_To_Deactivate_Template',N'هل أنت متاكد أنك تريد إلغاء تنشيط الحدث؟','Are you sure you want to deactivate the template?','Event List',1
------------- 16-4-2024 ---------------
EXEC [dbo].pInsTranslation 'View_Notification',N'عرض','View','Notification',1
EXEC [dbo].pInsTranslation 'Details_Notification',N'التفاصيل','Details','Notification',1
EXEC [dbo].pInsTranslation 'View_All_Notification',N'عرض الكل','View All','Notification',1
EXEC [dbo].pInsTranslation 'Update_Successfully',N'Update Successfully','Update Successfully','Notification',1
/*<History Author='Umer Zaman' Date='04-04-2024'> Script end </History>*/

/*<History Author='Umer Zaman' Date='17-04-2024'> Script start </History>*/
EXEC [dbo].pInsTranslation 'Submit_Workflow',N'إرسال آلية العمل','Submit Workflow','Create Workflow page',1
EXEC [dbo].pInsTranslation 'Workflow_Added_Successfully',N'تم ارسال آلية العمل بنجاح','Workflow has been submitted successfully','Create Workflow page',1
EXEC [dbo].pInsTranslation 'Mandatory_Template_Option_Error_Message',N'برجاء تحديد خيارات المقدمة والملاحظة التوضيحية والملاحظة من القالب','Please select Introduction, Explanatory Note and Note options from template','Create Legal Legislation page',1
/*<History Author='Umer Zaman' Date='17-04-2024'> Script end </History>*/
----------- 18-04-2024-----
EXEC [dbo].pInsTranslation 'z_Today',N'اليوم','Today','Notification',1
EXEC [dbo].pInsTranslation 'This Month',N'هذا الشهر','This Month','Notification',1
EXEC [dbo].pInsTranslation 'Last Month',N'الشهر الماضي','Last Month','Notification',1

/*<History Author='Umer Zaman' Date='04-04-2024'> Script end </History>*/


EXEC [dbo].pInsTranslation 'Read',N'يقرأ','Read','Notification',1
EXEC [dbo].pInsTranslation 'EventName_En',N'اسم الحدث','Event Name','Notification Event',1
EXEC [dbo].pInsTranslation 'EventName_Ar',N'اسم الحدث عربي','Event Name Arabic','Notification Event',1
EXEC [dbo].pInsTranslation 'ReceiverType',N'نوع جهاز الاستقبال','Receiver Type','Notification Event',1 
EXEC [dbo].pInsTranslation 'Edit_Notification_Event',N'تحرير حدث الإخطار','Edit Notification Event','Notification Event',1
EXEC [dbo].pInsTranslation 'Event_Updated_Successfully',N'تم تحديث حدث الإعلام بنجاح.','Notification Event Updated Successfully.','Notification Event',1
EXEC [dbo].pInsTranslation 'Sure_update_Event',N'هل أنت متأكد أنك تريد تحديث حدث الإشعارات؟','Are you sure you want to update Notification Event?','Notification Event',1
EXEC [dbo].pInsTranslation 'elderly',N'كبار السن','Older','Notification',1
EXEC [dbo].pInsTranslation 'OLD',N'السابقة','Old (Older than 30 days)','Notification',1
EXEC [dbo].pInsTranslation 'Yesterday',N'أمس','Yesterday','Notification',1
EXEC [dbo].pInsTranslation 'G2G_Tarasol_Correspondence',N'التراسل الإلكتروني الحكومي','G2G Tarasol Correspondence','communication Detail',1


-------18-04-2024---------
EXEC [dbo].pInsTranslation 'Last Month',N'الشهر السابق','Last Month','Notification',1
EXEC [dbo].pInsTranslation 'elderly',N'السابقة','Older','Notification',1
EXEC [dbo].pInsTranslation 'OLD',N'السابقة ( أكثر من 30 يوم )','Old (Older than 30 days)','Notification',1
EXEC [dbo].pInsTranslation 'View_all_notification_text',N'تود عرض جميع الزيارات، قم بزيارة','Want to view all Notification, Visit ','Notification',1
EXEC [dbo].pInsTranslation 'Notification_Page',N'صفحة الإشعارات','Notification Page','Notification',1
EXEC [dbo].pInsTranslation 'Sure_Want_To_Mark_All_Read',N'هل أنت متأكد أنك تريد اعتبار كل الإشعارات مقروءة؟','Are you sure you want to mark all notifications as read?','Notification',1
EXEC [dbo].pInsTranslation 'Update_Successfully',N'تم التعديل بنجاح','Updated Successfully','Notification',1

------- OSS ---------- 21-04-2024 -------- 

EXEC [dbo].pInsTranslation 'Item_Arbaic_Name',N'اسم العنصر باللغة العربية','Item Arbaic Name','Inventory Management system',1
EXEC [dbo].pInsTranslation 'Total_Quantity',N'الكمية الإجمالية','Total Quantity','Add Item',1
EXEC [dbo].pInsTranslation 'Unit',N'وحدة','Unit','Add Item',1
EXEC [dbo].pInsTranslation 'Item_Description',N'وصف السلعة','Item Description','Add Item',1
EXEC [dbo].pInsTranslation 'Expiry_Date',N'تاريخ الانتهاء','Expiry Date','Add Item',1
EXEC [dbo].pInsTranslation 'Quantity_Per_Unit',N'الكمية لكل وحدة','Quantity Per Unit','Add Item',1
EXEC [dbo].pInsTranslation 'Add_Store',N'إضافة متجر','Add Store','Add Store',1
EXEC [dbo].pInsTranslation 'Add_Floor_Store',N'إضافة متجر الكلمة','Add Floor Store','Add Store',1
EXEC [dbo].pInsTranslation 'Add_Main_Store',N'إضافة المتجر الرئيسي','Add Main Store','Add Store',1
EXEC [dbo].pInsTranslation 'Transfer_Items',N'نقل العناصر','Transfer Items','Transfer Items',1
EXEC [dbo].pInsTranslation 'Transfer',N'تحويل','Transfer','Transfer Items',1
EXEC [dbo].pInsTranslation 'Stores',N'متاجر','Stores','Transfer Items',1
EXEC [dbo].pInsTranslation 'Fatwa_Main_Store',N'متجر الفتوى الرئيسي','Fatwa Main Store','Add Item',1
EXEC [dbo].pInsTranslation 'Transfer_Item_Store',N'نقل مخزن العناصر','Transfer Item Store','Transfer Items',1
EXEC [dbo].pInsTranslation 'TransferItem_Rejected_Successfully',N'تم رفض عنصر النقل بنجاح','TransferItem Rejected Successfully','Transfer Items',1
EXEC [dbo].pInsTranslation 'TransferItem_Approved_Successfully',N'تمت الموافقة على نقل العنصر بنجاح','TransferItem Approved Successfully','Transfer Items',1
EXEC [dbo].pInsTranslation 'Transfer_Details',N'تفاصيل النقل','Transfer Details','Transfer Items',1
EXEC [dbo].pInsTranslation 'Transfer_Quantity',N'نقل الكمية','Transfer Quantity','Transfer Items',1
EXEC [dbo].pInsTranslation 'Transfer_By',N'نقل بواسطة','Transfer By','Transfer Items Detail',1
EXEC [dbo].pInsTranslation 'Transfer_Status',N'حالة نقل','Transfer Status','Transfer Items Detail',1
EXEC [dbo].pInsTranslation 'Unit',N'وحدة','Unit','Transfer Items Detail',1
EXEC [dbo].pInsTranslation 'Store_Already_Exist',N'المتجر موجود بالفعل','Store Already Exist','Add Store',1
EXEC [dbo].pInsTranslation 'Store_Successfully_Added',N'تمت إضافة المتجر بنجاح','Store Successfully Added','Add Store',1
EXEC [dbo].pInsTranslation 'Store_Successfully_Updated',N'تم تحديث المتجر بنجاح','Store Successfully Updated','Add Store',1
EXEC [dbo].pInsTranslation 'Failed_To_Update_Store_Details._Please_Try_Again',N'فشل في تحديث تفاصيل المتجر. حاول مرة اخرى','Failed To Update Store Details. Please Try Again','Add Store',1







EXEC [dbo].pInsTranslation 'Update_Successfully',N'تم التعديل بنجاح','Updated Successfully','Notification',1

------------- Legal Principle Table Scripts 2024-04-21 ---------------

EXEC [dbo].pInsTranslation 'Page_Number',N'رقم الصفحة','Page Number','Add legal principle page',1
EXEC [dbo].pInsTranslation 'Add_Relation_Page_Number',N'إضافة رقم الصفحة','Add Page Number','Add legal principle page',1
EXEC [dbo].pInsTranslation 'Atleast_One_Category',N'الرجاء تحديد فئة واحدة على الأقل','Please select atleast one category','Add legal principle page',1
EXEC [dbo].pInsTranslation 'Judgment_Document_Not_Loaded',N'تعذر تحميل مستند الحكم من المسار المصدر','Judgment document could not be loaded from source path.','Add legal principle page',1

------------- Legal Principle Table Scripts 2024-04-21 ---------------
/*<History Author='Umer Zaman' Date='04-04-2024'> Script end </History>*/


/*<History Author='Ammaar Naveed' Date='16-04-2024'>New translations </History>*/
EXEC [dbo].pInsTranslation 'Name_En',N'(الاسم (انجليزي','Name (English)','For whole application',1
EXEC [dbo].pInsTranslation 'Name_Ar',N'(الاسم (عربي','Name (Arabic)','For whole application',1
EXEC [dbo].pInsTranslation 'Grouptype_Name',N'اسم نوع المجموعة','Group Type Name','Add Group Access Type',1
EXEC [dbo].pInsTranslation 'Websystem_Name',N'اسم نظام الويب','Web System Name','Add Group Access Type',1
EXEC [dbo].pInsTranslation 'Communication',N'التواصل','Communication','Fatwa Admin Permission Grid',1
EXEC [dbo].pInsTranslation 'Legal_Library_System',N'نظام المكتبة القانونية','Legal Library System','Fatwa Admin Permission Grid',1
EXEC [dbo].pInsTranslation 'Document_Management_System',N'نظام إدارة المستندات','Document Management System','Fatwa Admin Permission Grid',1
EXEC [dbo].pInsTranslation 'Case_Consultation_Management_Shared',N'الصلاحيات المشتركة لإدارة القضايا/الاستشاري','Case/Consultation Management Shared Permissions','Fatwa Admin Permission Grid',1
EXEC [dbo].pInsTranslation 'Case_Consultation_Shared',N'الصلاحيات المشتركة لإدارة القضايا/الاستشاري','Case/Consultation Management Shared','Fatwa Admin Permission Grid',1
EXEC [dbo].pInsTranslation 'Inventory_Management_System',N'نظام إدارة المخازن','Inventory Management System','Fatwa Admin Permission Grid',1
EXEC [dbo].pInsTranslation 'Employee_Management_System',N'نظام إدارة الموظفين','Employee Management System','Fatwa Admin Permission Grid',1
EXEC [dbo].pInsTranslation 'System_User_Management',N'نظام إدارة المستخدمين','User Management System','Fatwa Admin Permission Grid',1
EXEC [dbo].pInsTranslation 'Shared_Common_Permissions',N'الصلاحيات العامة والمشتركة','Shared/Common Permissions','Fatwa Admin Permission Grid',1
EXEC [dbo].pInsTranslation 'Shared_Permissions',N'الصلاحيات المشتركة','Shared Permissions','Fatwa Admin Permission Grid',1
EXEC [dbo].pInsTranslation 'Shared_Permissions_Tab',N'بعض الصلاحيات مشتركة مع الأنظمة الأخرى. بعد تحديد الصلاحيات المراد بها، لا تنسى ان تحديد الصلاحيات من شاشة الصلاحيات المشتركة','Some of the permissions are common in each module. After assigning relevant permissions, do not forget to assign menu/dashboard permissions from the "Shared Permissions" tab.','Fatwa Admin Permission Grid',1
EXEC [dbo].pInsTranslation 'Note',N'ملاحظة','Note','Fatwa Admin Permission Grid',1
EXEC [dbo].pInsTranslation 'Current_And_Previous_Hearings',N'الجلسات الحالية والسابقة','Current & Previous Hearings','Case Management',1
EXEC [dbo].pInsTranslation 'Upcoming_Hearings',N'الجلسات القادمة','Upcoming Hearings','CMS',1
EXEC [dbo].pInsTranslation 'Employee_Activity',N'إجراءات الموظف','Employee Activity','User Activity Tracking',1
EXEC [dbo].pInsTranslation 'Computer_Name',N'اسم الكمبيوتر','Computer Name','User Activity Tracking',1
EXEC [dbo].pInsTranslation 'Logged_In',N'تم تسجيل الدخول','Logged In','User Activity Tracking',1
EXEC [dbo].pInsTranslation 'Not_Logged_In',N'لم يتم تسجيل الدخول','Not Logged In','User Activity Tracking',1
EXEC [dbo].pInsTranslation 'Login_Timestamp',N'الطابع الزمني لتسجيل الدخول','Login Timestamp','User Activity Tracking',1
EXEC [dbo].pInsTranslation 'Logged_Out',N'تم تسجيل الخروج','Logged Out','User Activity Tracking',1
EXEC [dbo].pInsTranslation 'Logout_Timestamp',N'الطابع الزمني لتسجيل الخروج','Logout Timestamp','User Activity Tracking',1
EXEC [dbo].pInsTranslation 'Exception',N'استثناء','Exception','User Activity Tracking',1
EXEC [dbo].pInsTranslation 'Group_Update_Dialog_Message',N'هل أنت متأكد أنك تريد تعديل هذه المجموعة؟','Are you sure you want to update this group?','Update User Group',1
EXEC [dbo].pInsTranslation 'Add_UMS_Group',N'هل أنت متأكد أنك تريد إضافة هذه المجموعة؟','Are you sure you want to add this group?','Update User Group',1
EXEC [dbo].pInsTranslation 'Assigned_Users',N'المستخدمين تم تعيينهم','Assigned Users','User Groups',1
EXEC [dbo].pInsTranslation 'Assign_Users_To_Group',N'تعيين مستخدمين جدد','Assign New Users','User Groups',1
EXEC [dbo].pInsTranslation 'On_Demand_Request_Portal',N'بوابة طلب معلومات من الجهة الحكومية','On Demand Request Portal','FATWA Admin Permission Grid',1
EXEC [dbo].pInsTranslation 'Group_Description',N'حدد الهدف من المجموعة','Specify the purpose of this group','Add User Group',1
EXEC [dbo].pInsTranslation 'Sector_Type_History',N'التغيرات على نوع القطاع','Sector Type History','Lookups',1
EXEC [dbo].pInsTranslation 'Edit_Sector_Detail',N'تعديل تفاصيل القطاع','Edit Sector Detail','Lookups',1
EXEC [dbo].pInsTranslation 'Activate_Confirm_Message',N'هل أنت متأكد أنك تريد تنشيط هذا المستخدم؟','Are you sure you want to activate this employee?','Edit Employee',1
EXEC [dbo].pInsTranslation 'Deactivate_Employee',N'إلغاء تنشيط المستخدم','Deactivate Employee','Edit Employee',1
EXEC [dbo].pInsTranslation 'Activate_Employee',N'تنشيط المستخدم','Activate Employee','Edit Employee',1

/*<History Author='Ammaar Naveed' Date='17-04-2024'>New & updated translations </History>*/
UPDATE tTranslation SET Value_En='Legal Legislation Management System' WHERE TranslationKey='Legislation_Management_System'
UPDATE tTranslation SET Value_En='Legal Library Management System' WHERE TranslationKey='Library_Management_System'
UPDATE tTranslation SET Value_En='Main Menu' WHERE TranslationKey='FATWA_Dashboard'
UPDATE tTranslation SET Value_Ar='Main Menu' WHERE TranslationKey='FATWA_Dashboard'
EXEC [dbo].pInsTranslation 'Sidemenu',N'Menu Items & Main Menu Screen','Menu Items & Main Menu Screen','Permissions Grid',1

/*<History Author='Ammaar Naveed' Date='18-04-2024'>New & updated translations </History>*/
UPDATE tTranslation SET Value_En='Some of the permissions are common in each module. After assigning relevant permissions, make sure to assign menu/dashboard permissions from the "Shared Permissions" tab.' WHERE TranslationKey='Shared_Permissions_Tab'
EXEC [dbo].pInsTranslation 'View_all_notification_text',N'Want to view all Notification, Visit ','Want to view all Notification, Visit ','Notification',1
EXEC [dbo].pInsTranslation 'Notification_Page',N'Notification Page','Notification Page','Notification',1
------------ 18-4-2024 --------------
EXEC [dbo].pInsTranslation 'Create_Notification_Event_Template',N'Create Notification Template','Create Notification Template','Create Template Page',1


------- 25-04-2024  -----
EXEC [dbo].pInsTranslation 'Display_Suggested_Principle',N'عرض المبدأ المقترح','Display Suggested Principle','LLS Legal Principle Add Form',1
EXEC [dbo].pInsTranslation 'Display_Suggested_Principle_Submit_Message',N'هل أنت متأكد من ارسال الوثيقة؟','Are you sure you want to submit?','LLS Legal Principle Add Form',1
EXEC [dbo].pInsTranslation 'Suggested_Principle',N'المبدأ المقترح','Suggested Principle','LLS Legal Principle Add Form',1
EXEC [dbo].pInsTranslation 'Legislation_Issue_Date',N'تاريخ الإصدار','Issue Date','Legal Legislation Add Form',1
----25-04-2024 ------

-------------------------------------- Author = Muhammad Ismail Date 25 April 2024 ------- OSS

 EXEC [dbo].pInsTranslation 'Store_Detail',N'تفاصيل المتجر','Store Detail','Store Detail',1
 EXEC [dbo].pInsTranslation 'Item_Category_Successfully_Updated',N'تم تحديث فئة العنصر بنجاحر','Item Category Successfully Updated','Edit Category',1
 EXEC [dbo].pInsTranslation 'Item_Category_Successfully_Added',N'تمت إضافة فئة العنصر بنجاح','Item Category Successfully Added','Add Category',1
 EXEC [dbo].pInsTranslation 'Item_Category_Already_Exist',N'فئة العنصر موجودة بالفعل','Item Category Already Exist','Add Category',1
 EXEC [dbo].pInsTranslation 'Add_Category',N'إضافة فئة','Add Category','Add Category',1
 EXEC [dbo].pInsTranslation 'Edit_Category',N'تحرير_الفئة','Edit Category','Edit Category',1
 EXEC [dbo].pInsTranslation 'No_Store_Associated_Contact_Admiistrator',N'لا يوجد مسؤول اتصال مرتبط بالمتجر','No Store Associated Contact Admiistrator','Transfer Items',1
 EXEC [dbo].pInsTranslation 'List_Of_Transfer_Items',N'قائمة عناصر النقل','List Of Transfer Items','Transfer Items',1
 EXEC [dbo].pInsTranslation 'List_Of_Category',N'قائمة الفئة','List Of Category','Category',1
 EXEC [dbo].pInsTranslation 'Add_New_Category',N'إضافة فئة جديدة','Add New Category','Category',1


 -- 29-04-2024 ----

EXEC [dbo].pInsTranslation 'No_Sector_and_department_against_this_user',N'لا يوجد قطاع أو قسم ضد هذا المستخدم.','There is no sector and department against this user.','LMS',1
EXEC [dbo].pInsTranslation 'Apply_Extension',N'تطبيق التمديد','Apply Extension','LMS Borrow List Page',1
EXEC [dbo].pInsTranslation 'Sure_Extend_The_Record',N'هل أنت متأكد أنك تريد تطبيق ملحق الكتاب.','Are you sure you want to apply the book extension.','LMS Borrow List Page',1
EXEC [dbo].pInsTranslation 'Extended_Success',N'کتاب کی توسیع کے لیے کامیابی کے ساتھ درخواست دی گئی ہے۔','Book has been applied for extension successfully.','LMS Borrow List Page',1

 -- 29-04-2024 ----

 /*<History Author='Ammaar Naveed' Date='01-05-2024'>Employee leave delegation translations.</History>*/
EXEC [dbo].pInsTranslation 'Select_Delegated_Employee',N'اختر الموظف المفوض','Select Delegated Employee','Employee Leave Delegation',1
EXEC [dbo].pInsTranslation 'Delegated_Employee_Added_Successfully',N'تم اضافة الموظف المفوض بنجاح','Delegated employee has been added successfully','Employee Leave Delegation',1
EXEC [dbo].pInsTranslation 'Delegated_Employee_Already_Added',N'تم اضافة الموظف المفوض مسبقا','The delegated employee has already added.','Employee Leave Delegation',1
EXEC [dbo].pInsTranslation 'Delegate_An_Employee',N'الموظف المفوض','Delegate an Employee','Employee Leave Delegation',1
EXEC [dbo].pInsTranslation 'Employee_Leave_Delegation_Record',N'سجل تفويض إجازة الموظف','Employee Leave Delegation Record','Employee Leave Delegation',1

 /*<History Author='Ammaar Naveed' Date='06-05-2024'>Employee leave delegation translations.</History>*/
EXEC [dbo].pInsTranslation 'ToDate_NotGreate_FromDate',N'لغاية التاريخ ما يقدر يكون أقل من تاريخ البداية','To Date should not be smaller than From Date','Employee Leave Delegation',1
EXEC [dbo].pInsTranslation 'Change_Dates',N'تغيير التواريخ','Change Dates','Employee Leave Delegation',1




---06-05-2024 ---

 /*<History Author='Muhammad Ali' Date='06-05-2024'>Worker Services and Admin panel translations.</History>*/
 EXEC [dbo].pInsTranslation 'Notifications_Event', N'أحداث الإشعارات','Notifications Event','Notifications',1
EXEC [dbo].pInsTranslation 'Dynamic_Lookups',N'القوائم المتغيرة','Dynamic Lookups','Dynamic Lookups',1
EXEC [dbo].pInsTranslation 'User_Management_Lookups',N'قوائم نظام إدارة المستخدمين','User Management Lookups','User Management Lookups',1
EXEC [dbo].pInsTranslation 'EP_Nationality',N'الجنسية','EP Nationality','Dynamic Lookups',1
EXEC [dbo].pInsTranslation 'EP_Grade',N'الدرجة الوظيفية','EP Grade','Dynamic Lookups',1
EXEC [dbo].pInsTranslation 'EP_Grade_Type',N'نوع الدرجة الوظيفية','EP Grade Type','Dynamic Lookups',1
EXEC [dbo].pInsTranslation 'EP_Designation',N'المسمى الوظيفي','EP Designation','Dynamic Lookups',1

EXEC [dbo].pInsTranslation 'Legal_Library_Lookups',N'قوائم نظام المكتبة القانونية','Legal Library Lookups','Dynamic Lookups',1
EXEC [dbo].pInsTranslation 'Book_Tags',N'Book Tags','وسوم الكتاب','Dynamic Lookups',1
EXEC [dbo].pInsTranslation 'Legislation_Lookups',N'قوائم نظام إدارة التشريعات','Legislation Lookups','Dynamic Lookups',1
EXEC [dbo].pInsTranslation 'Legal_Principle_Lookups',N'قوائم نظام إدارة المبادئ القانونية','Legal Principle Lookups','Dynamic Lookups',1
EXEC [dbo].pInsTranslation 'Tags_No',N'رقم الوسم','Tags No','Dynamic Lookups',1
EXEC [dbo].pInsTranslation 'Book_Author',N'مؤلفي الكتب','Book Author','Dynamic Lookups',1
EXEC [dbo].pInsTranslation 'Fixed_Lookups',N'القوائم الثابتة','Fixed Lookups','Fixed Lookups',1
EXEC [dbo].pInsTranslation 'Enums_Lists',N'قوائم الإعدادات','Enums Lists','Enums',1

EXEC [dbo].pInsTranslation 'Execution_Status',N'حالة ملف التنفيذ','Execution Status','Worker Services',1
EXEC [dbo].pInsTranslation 'Activity_Name',N'اسم الاجراء','Activity Name','Worker Services',1
EXEC [dbo].pInsTranslation 'Communication_Type',N'نوع التواصل','Communication Type','Worker Services',1
EXEC [dbo].pInsTranslation 'SLA_Interval',N'المدد الزمنية','SLA Interval','Worker Services',1
EXEC [dbo].pInsTranslation 'Worker_Service_Execution_Log',N'سجلات تنفيذ الخدمات','Worker Service Execution Log','Worker Services',1
EXEC [dbo].pInsTranslation 'Worker_Service_Execution_Log_Detail',N'تفاصيل سجل تنفيذ الخدمات','Worker Service Execution Log Detail','Worker Services',1
EXEC [dbo].pInsTranslation 'Execution_Time',N'وقت التنفيذ','Execution Time','Worker Services',1
EXEC [dbo].pInsTranslation 'Last_Modified',N'اخر تحديث','Last Modified','Worker Services',1
EXEC [dbo].pInsTranslation 'Reattempt_Count',N'عدد مرات إعادة المحاولة','Reattempt Count','Worker Services',1
EXEC [dbo].pInsTranslation 'End_Date_Time',N'تاريخ ووقت النهاية','End Date Time','Worker Services',1
EXEC [dbo].pInsTranslation 'Start_Date_Time',N'تاريخ ووقت البداية','Start Date Time','Worker Services',1
EXEC [dbo].pInsTranslation 'Worker_Service',N'الخدمات','Worker Service','Worker Services',1
EXEC [dbo].pInsTranslation 'Holiday_Date',N'تواريخ العطل','Holiday Date','Worker Services',1


EXEC [dbo].pInsTranslation 'Consultation_Legislation_File_Type(Fatwa&G2G)',N'أنواع ملف التشريع','Consultation Legislation File Type(Fatwa&G2G)','accordion names',1
EXEC [dbo].pInsTranslation 'Coms_International_Arbitration(Fatwa&G2G)',N'أنواع التحكيم الدولي','Coms International Arbitration(Fatwa&G2G)','accordion names',1
EXEC [dbo].pInsTranslation 'Request_Type_Fatwa/G2G',N'أنواع الطلب','Request Type Fatwa/G2G','accordion names',1
EXEC [dbo].pInsTranslation 'Case_Management_Lookups',N'قوائم نظام إدارة القضايا','Case Management Lookups','accordion names',1
EXEC [dbo].pInsTranslation 'Fatwa/G2G_Courts',N'أسماء المحاكم','Fatwa/G2G Courts','accordion names',1
EXEC [dbo].pInsTranslation 'Fatwa/G2G_Chambers',N'أسماء الدوائر','Fatwa/G2G Chambers','accordion names',1
EXEC [dbo].pInsTranslation 'Chambers_Hearings_Days',N'أيام جلسات الدوائر','Chambers Hearings Days','accordion namess',1
EXEC [dbo].pInsTranslation 'Request_Type_Fatwa/G2G',N'أنواع الطلب','Request Type Fatwa/G2G','accordion names',1
EXEC [dbo].pInsTranslation 'Case_File_Status_Fatwa/G2G',N'حالة ملف القضية','Case File Status Fatwa/G2G','accordion names',1

---06-05-2024 ---
EXEC [dbo].pInsTranslation 'Temp_Portfolio_Opening_Statement',N'يرجى الاطلاع على المستندات التالية المضافة في محفظة المستندات:','Please see the following documents added in the document portfolio:','Document Portfolio',1
 -- 07-05-2024 start ----
EXEC [dbo].pInsTranslation 'Principle_Category_Section',N'قسم فئة المبدأ','Principle Category Section','Legal Principle Add Form Page',1

 -- 07-05-2024 end ----

/*<History Author='Ammaar Naveed' Date='12-05-2024'>Missing/New translations</History>*/
EXEC [dbo].pInsTranslation 'Sector_Name',N'اسم القطاع','Sector Name','Add Group',1

/*<History Author='Muhammad Ali' Date='15-05-2024'>Missing/New translations</History>*/
EXEC [dbo].pInsTranslation 'Chambers_G2G_And_Fatwa',N'أسماء الدوائر','Fatwa/G2G Chambers','accordion names',1
EXEC [dbo].pInsTranslation 'Dynamic_Lookups',N'القوائم المتغيرة','Dynamic Lookups','Dynamic Lookups',1
EXEC [dbo].pInsTranslation 'Book_Tags',N'وسوم الكتاب','Book Tags','Dynamic Lookups',1
EXEC [dbo].pInsTranslation 'Request_Type_G2G_And_Fatwa',N'أنواع الطلب','Request Type Fatwa/G2G','accordion names',1
EXEC [dbo].pInsTranslation 'Case_File_Status_G2G_And_Fatwa',N'حالة ملف القضية','Case File Status Fatwa/G2G','accordion names',1
EXEC [dbo].pInsTranslation 'Consultation_Legislation_File_Type_G2G_And_Fatwa',N'أنواع ملف التشريع','Consultation Legislation File Type(Fatwa&G2G)','accordion names',1
EXEC [dbo].pInsTranslation 'Coms_International_Arbitration_G2G_And_Fatwa',N'أنواع التحكيم الدولي','Coms International Arbitration(Fatwa&G2G)','accordion names',1


/*<History Author='Muhammad Ali' Date='15-05-2024'>Missing/New more translations</History>*/
EXEC [dbo].pInsTranslation 'Add_Literature_Dewey_Number_Pattern',N'إضافة نمط رقم الديوي','Add Literature Dewey Number Pattern','Dewey Number Pattern',1
EXEC [dbo].pInsTranslation 'Edit_Literature_Dewey_Number_Pattern',N'تعديل نمط رقم الديوي','Edit Literature Dewey Number Pattern','Dewey Number Pattern',1
EXEC [dbo].pInsTranslation 'Are_you_Sure_You_Want_to_Delete_Literature_Dewey_Number_Pattern',N'هل أنت متأكد أنك تريد حذف نمط رقم الديوي؟','Are you Sure You Want to Delete Literature Dewey Number Pattern?','Dewey Number Pattern',1
EXEC [dbo].pInsTranslation 'Pattern_Type',N'نوع النمط','Pattern Type','Dewey Number Pattern',1
EXEC [dbo].pInsTranslation 'Series_Number',N'رقم الجزء','Series Number','Dewey Number Pattern',1
EXEC [dbo].pInsTranslation 'Digit_Sequence_Number',N'رقم التسلسل','Digit Sequence Number','Dewey Number Pattern',1
EXEC [dbo].pInsTranslation 'Literature_Dewey_Number_Pattern_Added_Successfully',N'تم إضافة نمط رقم الديوي بنجاح','Literature Dewey Number Pattern Added Successfully','Dewey Number Pattern',1
EXEC [dbo].pInsTranslation 'Could_not_create_a_new_Literature_Dewey_Number_Pattern',N'تعذر إضافة نمط رقم الديوي','Could not create a new Literature Dewey Number Pattern','Dewey Number Pattern',1
EXEC [dbo].pInsTranslation 'Literature_Dewey_Number_Pattern_could_not_be_updated',N'تعذر تعديل نمط رقم الديوي','Literature Dewey Number Pattern could not be updated','Dewey Number Pattern',1
EXEC [dbo].pInsTranslation 'Literature_Dewey_Number_Pattern_Updated_Successfully',N'تم تعديل نمط رقم الديوي بنجاح','Literature Dewey Number Pattern Updated Successfully','Dewey Number Pattern',1
EXEC [dbo].pInsTranslation 'Consultation_Management_Lookups',N'إعدادات نظام الاستشاري','Consultation Management Lookups','Consultation',1

-----15 may 2024----
EXEC [dbo].pInsTranslation 'View_Principle_Details',N'عرض تفاصيل المبدأ','View Principle Detail','Legal Principle',1
EXEC [dbo].pInsTranslation 'Edit_Principle_Content',N'تحرير محتوى المبدأ','Edit Principle Content','Legal Principle',1



---- LLS legal principle 16-05-2024

EXEC [dbo].pInsTranslation 'Are_you_Sure_You_Want_to_Update_Record',N'هل أنت متأكد أنك تريد تحديث السجل','Are you sure you want to update the record?','LLS legal principle add form page',1
EXEC [dbo].pInsTranslation 'Please_Add_Atleast_One_Principle_Content',N'الرجاء إضافة محتوى أساسي واحد على الأقل','Please add at least one principle content','LLS legal principle add form page',1
EXEC [dbo].pInsTranslation 'Add_Principle_Content_To_Grid',N'إضافة محتوى المبدأ','Add principle content','LLS legal principle add form page',1
EXEC [dbo].pInsTranslation 'Principle_Category_Section',N'قسم فئة المبدأ','Principle Category Section','Legal Principle Add Form Page',1


----- LLS legal principle 16-05-2024

/*<History Author='Ammaar Naveed' Date='19-05-2024'>New translations</History>*/
EXEC [dbo].pInsTranslation 'Sector_Users',N'مستخدمين القطاع','Sector Users','CMS Sector Users',1

--- Literature 20-05-2024 start
EXEC [dbo].pInsTranslation 'Update_Principle_Content_To_Grid',N'تحديث محتوى المبدأ','Update principle content','LLS legal principle add form page',1
EXEC [dbo].pInsTranslation 'Principle_Category_Section',N'قسم الفئة','Category Section','LLS legal principle add form page',1
EXEC [dbo].pInsTranslation 'Borrow_Another_Book_Confirmation',N'طلب كتاب آخر؟','Request Another Book?','Lms Literature borrow add form page',1
EXEC [dbo].pInsTranslation 'Borrow_Another_Book',N'هل تريد استعارة كتاب آخر؟','Would you like to borrow another book?','Lms Literature borrow add form page',1


--- Literature 20-05-2024 end

----25-04-2024 ------

/*<History Author='Arshad khan' Date='19-05-2024'>New  translations </History>*/


EXEC [dbo].pInsTranslation 'Bug_Type_Lookups',N'قوائم أنواع الأخطاء','Bug Type Lookups','Side Menu',1

EXEC [dbo].pInsTranslation 'Bug_Type_Lookups',N'Issue Type Lookups','Issue Type Lookups','Side Menu',1

EXEC [dbo].pInsTranslation 'Bug_Tickets',N'الطلب','Ticket','Side Menu',1

EXEC [dbo].pInsTranslation 'Reported_Bugs',N'الأخطاء المبلغ عنها','Reported Bugs','Side Menu',1

EXEC [dbo].pInsTranslation 'Bug_Reporting',N'الإبلاغ عن الأخطاء','Bug Reporting','Side Menu',1

EXEC [dbo].pInsTranslation 'Sure_Remove_Bug_Type',N'هل أنت متأكد أنك تريد حذف نوع الأخطاء؟','Are you sure you want to remove the bug type?','List Bug Type',1

EXEC [dbo].pInsTranslation 'Bug_Type_Removed_Successfully',N'تم حذف نوع الخطأ بنجاح','Bug type removed successfully','List Bug Type',1

EXEC [dbo].pInsTranslation 'Bug_Type_Added_Successfully',N'تم إضافة نوع الخطأ بنجاح','Bug type added successfully','Add Bug Type',1

EXEC [dbo].pInsTranslation 'Add_Bug_type',N'إضافة نوع الخطأ','Add Bug Type','Add Bug Type',1

EXEC [dbo].pInsTranslation 'List_Bug_Type',N'قائمة أنواع الخطأ','List Bug Type','List Bug Type',1

EXEC [dbo].pInsTranslation 'Reported_Bug_Details',N'تفاصيل الأخطاء المبلغ عنها','Reported Bug Detail','Detail Reported Bug',1

EXEC [dbo].pInsTranslation 'Bug_Type',N'نوع الخطأ','Bug Type','View Crash Report',1

EXEC [dbo].pInsTranslation 'Crash_Report',N'تقرير الأعطال','Crash Report','View Crash Report',1

EXEC [dbo].pInsTranslation 'Bug_Reporting_Date',N'الإبلاغ عن الأخطاء','Bug Reporting Date','Detail Bug Ticket',1

EXEC [dbo].pInsTranslation 'Bug_Ticket_Updated_Successfully',N'تم تعديل طلب الإبلاغ عن الأخطاء بنجاح','Bug ticket updated successfully','Add Ticket',1

EXEC [dbo].pInsTranslation 'Bug_Ticket_Added_Successfully',N'تم إضافة طلب الإبلاغ عن الأخطاء بنجاح','Bug ticket added successfully','Add Ticket',1

EXEC [dbo].pInsTranslation 'Ticket_Number',N'رقم الطلب','Ticket Number','Add Ticket',1

EXEC [dbo].pInsTranslation 'Update_Bug_Ticket',N'تعديل طلب الإبلاغ عن الأخطاء','Update Bug Ticket','Add Ticket',1

EXEC [dbo].pInsTranslation 'Bug_Updated_Successfully',N'تم تعديل الإبلاغ عن الأخطاء بنجاح','Bug updated successfully','Add Reported Bug',1

EXEC [dbo].pInsTranslation 'Add_Bug_Ticket',N'إضافة طلب الإبلاغ عن الأخطاء','Add Bug Ticket','Add Ticket',1


EXEC [dbo].pInsTranslation 'Bug_Ticket_Draft_Added_Successfully',N'تمت إضافة الطلب كمسودة بنجاح','Ticket Added as Draft Successfully','Add Bug Type',1

EXEC [dbo].pInsTranslation 'Close_All_Tickets',N'إغلاق جميع الطلبات','Close All Tickets','Add Ticket',1

EXEC [dbo].pInsTranslation 'Close_Ticket',N'إغلاق الطلب','Close Ticket','Detail',1

EXEC [dbo].pInsTranslation 'list-crashreport',N'قائمة تقرير الأعطال','List CrashReport','List Crash Report',1

EXEC [dbo].pInsTranslation 'Ticket_Closed_Successfully',N'تم إغلاق الطلب بنجاح','Ticket has been Closed Successfully','Add Bug Type',1

EXEC [dbo].pInsTranslation 'Item_History',N'سجل الأعمال','History','Common',1

EXEC [dbo].pInsTranslation 'History',N'سجل الأعمال','History','Common',1

EXEC [dbo].pInsTranslation 'Bug_Tickets',N'الطلب','Ticket','Side Menu',1

EXEC [dbo].pInsTranslation 'Reported_Bugs',N'الأخطاء المبلغ عنها','Reported Bugs','Side Menu',1

EXEC [dbo].pInsTranslation 'Bug_Reporting',N'الإبلاغ عن الأخطاء','Bug Reporting','Side Menu',1

EXEC [dbo].pInsTranslation 'Sure_Remove_Bug_Type',N'هل أنت متأكد أنك تريد حذف نوع الأخطاء؟','Are you sure you want to remove the type?','List Bug Type',1

EXEC [dbo].pInsTranslation 'Bug_Type_Removed_Successfully',N'تم حذف نوع الخطأ بنجاح','Type removed successfully','List Bug Type',1

EXEC [dbo].pInsTranslation 'Bug_Type_Added_Successfully',N'تم إضافة نوع الخطأ بنجاح','Type added successfully','Add Bug Type',1

EXEC [dbo].pInsTranslation 'Add_Bug_type',N'إضافة نوع الخطأ','Add Type','Add Bug Type',1

EXEC [dbo].pInsTranslation 'List_Bug_Type',N'قائمة أنواع الأخطاء','List Type','List Type',1

EXEC [dbo].pInsTranslation 'Reported_Bug_Details',N'تفاصيل الأخطاء المبلغ عنها','Reported Bug Detail','Detail Reported Bug',1

EXEC [dbo].pInsTranslation 'Bug_Type',N'نوع الخطأ','Bug Type','View Crash Report',1

EXEC [dbo].pInsTranslation 'Crash_Report',N'تقرير الأعطال','Crash Report','View Crash Report',1

EXEC [dbo].pInsTranslation 'Bug_Reporting_Date',N'الإبلاغ عن الأخطاء','Bug Reporting Date','Detail Bug Ticket',1

EXEC [dbo].pInsTranslation 'Bug_Ticket_Updated_Successfully',N'تم تعديل طلب الإبلاغ عن الأخطاء بنجاح','Ticket updated successfully','Add Ticket',1

EXEC [dbo].pInsTranslation 'Bug_Ticket_Added_Successfully',N'تم إضافة طلب الإبلاغ عن الأخطاء بنجاح','Ticket added successfully','Add Ticket',1

EXEC [dbo].pInsTranslation 'Ticket_Number',N'رقم الطلب','Ticket Number','Add Ticket',1

EXEC [dbo].pInsTranslation 'Update_Bug_Ticket',N'تعديل طلب الإبلاغ عن الأخطاء','Update  Ticket','Add Ticket',1

EXEC [dbo].pInsTranslation 'Bug_Updated_Successfully',N'تم تعديل الإبلاغ عن الأخطاء بنجاح','Bug updated successfully','Add Reported Bug',1

EXEC [dbo].pInsTranslation 'ReOpen_Reason',N'سبب إعادة الفتح','Reason To Reopen','view bugticket',1

EXEC [dbo].pInsTranslation 'Ticket_Tag_Successfully',N'تم وسم الطلب الطلب بنجاح','Ticket Tag Successfully','view bugticket',1

EXEC [dbo].pInsTranslation 'Add_Bug_Ticket',N'إضافة طلب الإبلاغ عن الأخطاء','Add Bug Ticket','Add Ticket',1

EXEC [dbo].pInsTranslation 'Tag_Ticket_To_Bug',N'توسيم طلب إلى خطأ','Tag Ticket To Bug','view bugticket',1

EXEC [dbo].pInsTranslation 'Bug_Added_Successfully',N'تم إضافة بلاغ الأخطاء بنجاح','Bug added successfully','Add Reported Bug',1

EXEC [dbo].pInsTranslation 'Max_TwoFifty_Characters',N'يجب أن يكون 250 حرفا','Must be 250 characters.','Add Reported Bug',1

EXEC [dbo].pInsTranslation 'Screen_Name_OR_Url',N'إسم النافذة / رابط النافذة','Screen Name/Screen Url','Add Reported Bug',1

EXEC [dbo].pInsTranslation 'Bug_Number',N'رقم الخطأ','Bug Number','Add Reported Bug',1

EXEC [dbo].pInsTranslation 'List_Reported_Bug',N'قائمة الأخطاء المبلغ عنها','List Reported Bug','List Reported Bug',1

EXEC [dbo].pInsTranslation 'Are_you_Sure_You_Want_to_Delete_Bug_Ticket_Comments',N'هل أنت متأكد من حذف تعليق الطلب؟','Are you Sure You Want to Delete Ticket Comment','view bugticket',1

EXEC [dbo].pInsTranslation 'Screen_Name',N'إسم النافذة','Screen Name','view bugticket',1 


EXEC [dbo].pInsTranslation 'FeedBack_Added_Successfully',N'تمت إضافة الملاحظة بنجاح','FeedBack Added Successfully','view bugticket',1

EXEC [dbo].pInsTranslation 'Comment_Added_Successfully',N'تمت إضافة التعليق بنجاح','Comment Added Successfully','view bugticket',1

EXEC [dbo].pInsTranslation 'Sure_Submit_FeedBack',N'هل أنت متأكد أنك تريد إرسال الملاحظة؟','Are you sure you want to submit FeedBack?','view bugticket',1

EXEC [dbo].pInsTranslation 'Sure_Submit_Comment',N'هل أنت متأكد أنك تريد إرسال التعليق؟','Are you sure you want to submit Comment?','view bugticket',1

EXEC [dbo].pInsTranslation 'List_Bug_Ticket',N'لائحة الطلبات','Ticket List','Add Ticket',1

EXEC [dbo].pInsTranslation 'FeedBack',N'ملاحظة ','FeedBack','view bugticket',1

EXEC [dbo].pInsTranslation 'Add_Feedback',N'إضافة ملاحظة','Add Feedback','view bugticket',1

EXEC [dbo].pInsTranslation 'Add_Comment',N'إضافة تعليق','Add Comment','view bugticket',1

EXEC [dbo].pInsTranslation 'Primary_Bug_Id',N'رقم المشكلة الرئيسية','Primary Bug Id','view bugticket',1

EXEC [dbo].pInsTranslation 'Reported_By',N'تم التقرير عنها من قبل','Reported By','list bugticket',1

EXEC [dbo].pInsTranslation 'Resolution_Date',N'تاريخ الحل','Resolution Date','list bugticket',1

EXEC [dbo].pInsTranslation 'Modification_Date',N'تاريخ التعديل','Modification Date','list bugticket',1

EXEC [dbo].pInsTranslation 'Severity',N'مدى الخطورة','Severity','list bugticket',1

EXEC [dbo].pInsTranslation 'Issue_Type',N'نوع المشكلة','Issue Type','list bugticket',1

EXEC [dbo].pInsTranslation 'Application',N'تطبيق','Application','list bugticket',1

EXEC [dbo].pInsTranslation 'TicketId',N'رقم الطلب','Ticket Id','list bugticket',1



EXEC [dbo].pInsTranslation 'UnAssign_BugType_User',N'إلغاء تعيين نوع المشكلة للمستخدم','UnAssign  Type To User','UnAssign Bug Type To User',1

EXEC [dbo].pInsTranslation 'Assign_BugType_User',N'تعيين نوع المشكلة للمستخدم','Assign  Type To User','Assign Bug Type To User',1

EXEC [dbo].pInsTranslation 'Un_Assign',N'إلغاء التعيين','UnAssign','Assign Type To User',1

EXEC [dbo].pInsTranslation 'Assign',N'تعيين','Assign','Assign Type To User',1

EXEC [dbo].pInsTranslation 'Update_Bug',N'تعديل تقرير الخطأ','Update Bug','Report A Bug',1

EXEC [dbo].pInsTranslation 'Assigned_User',N'المستخدمين تم تعيينهم','Assigned User','Detail Ticket',1

EXEC [dbo].pInsTranslation 'Accept',N'قبول','Accept','view bugticket',1

















---------------------------------------------------------------------------------------------------------------------------------------------------
---------------------------------------------------------------------------------------------------------------------------------------------------





EXEC [dbo].pInsTranslation 'Hearing_Lawyer',N'العضو حاضر الجلسة','Lawyer','CMS',1
EXEC [dbo].pInsTranslation 'Mark_As_Important',N'Mark As Important','Mark As Important','CMS',1
EXEC [dbo].pInsTranslation 'Mark_As_Un_Important',N'Mark As Unimportant','Mark As Unimportant','CMS',1
/*<History Author='Ammaar Naveed' Date='21-05-2024'>New translations</History>*/
EXEC [dbo].pInsTranslation 'Total_Cases',N'إجمالي الحالات','Total Cases','Assign A Lawyer Dropdown',1
EXEC [dbo].pInsTranslation 'Current_Cases',N'الحالات الحالية','Current Cases','Assign A Lawyer Dropdown',1

--- LLS Legal Prinmciple 26-05-2024 start


EXEC [dbo].pInsTranslation 'Add_Principle_Content_To_Grid',N'إضافة محتوى المبدأ','Add principle content','LLS legal principle add form page',1
EXEC [dbo].pInsTranslation 'Edit_Label',N'تحرير العقدة','Edit Node','LLS Legal Principle Add form page',1
EXEC [dbo].pInsTranslation 'Remove_Node',N'إزالة العقدة','Remove Node','LLS Legal Principle Add form page',1
EXEC [dbo].pInsTranslation 'Add_SubCategory',N'إضافة عقدة فرعية','Add Sub Node','LLS Legal Principle Add form page',1


--- LLS Legal Prinmciple 26-05-2024 end

--- LLS Literature Lookups 27-05-2024 start

EXEC [dbo].pInsTranslation 'Seperator_Pattern',N'نمط الفاصل','Seperator Pattern','Literature Pattern',1
EXEC [dbo].pInsTranslation 'Cheracter_Series',N'سلسلة الشخصيات','Cheracter Series','Literature Pattern',1
EXEC [dbo].pInsTranslation 'Series_Sequence_Number',N'الرقم التسلسلي للسلسلة','Series Sequence Number','Literature Pattern',1
EXEC [dbo].pInsTranslation 'cheracter_Sequence_Order_Not_Equal_To_Series_Sequence_Order',N'يجب ألا يكون ترتيب تسلسل الأحرف مساوياً لترتيب تسلسل السلسلة','cheracter Sequence Order Not Equal To Series Sequence Order','Literature Pattern',1

---------------- end
--- LLS Legal Prinmciple 26-05-2024 end


EXEC [dbo].pInsTranslation 'Document_Portfolio_Sr',N'مسلسل','Sr No.','Document Portfolio',1
EXEC [dbo].pInsTranslation 'Document_Portfolio_Document_Date',N'تاريخ المستند','Document Date','Document Portfolio',1
EXEC [dbo].pInsTranslation 'Document_Portfolio_No_Of_Pages',N'عدد اوراقة','No of Pages','Document Portfolio',1
EXEC [dbo].pInsTranslation 'Document_Portfolio_FileName',N'بيان المستند','File Name','Document Portfolio',1
EXEC [dbo].pInsTranslation 'Document_Portfolio_Portfolio',N'حافظة مستندات','Document Portfolio','Document Portfolio',1
EXEC [dbo].pInsTranslation 'Document_Portfolio_Introduction',N'مقدمة من','Introduction','Document Portfolio',1
EXEC [dbo].pInsTranslation 'Document_Portfolio_Mr',N'السيد/ ','Mr. ','Document Portfolio',1
EXEC [dbo].pInsTranslation 'Document_Portfolio_Capacity',N'بصفته','in his capacity','Document Portfolio',1
EXEC [dbo].pInsTranslation 'Document_Portfolio_Aged',N'الطاعن','aged','Document Portfolio',1
EXEC [dbo].pInsTranslation 'Document_Portfolio_Against',N'ضد','Against','Document Portfolio',1
EXEC [dbo].pInsTranslation 'Document_Portfolio_Appellant',N'المطعون ضده','Appellant','Document Portfolio',1
EXEC [dbo].pInsTranslation 'Document_Portfolio_Appeal_No',N'في الطعن رقم: ','In Appeal No: ','Document Portfolio',1
EXEC [dbo].pInsTranslation 'Document_Portfolio_Session',N'جلسة: ','Session: ','Document Portfolio',1
EXEC [dbo].pInsTranslation 'Document_Portfolio_Session_Under_Appointment',N'جلسة تحت التحديد','Session under appointment','Document Portfolio',1
EXEC [dbo].pInsTranslation 'Document_Portfolio_Member_Fatwa',N'عضو الفتوى والتشريع','Member of Fatwa and Legislation','Document Portfolio',1
EXEC [dbo].pInsTranslation 'Document_Portfolio_Document',N'مستند','Document','Document Portfolio',1
EXEC [dbo].pInsTranslation 'Document_Portfolio_From',N' من ',' of ','Document Portfolio',1
EXEC [dbo].pInsTranslation 'Document_Portfolio_Sheets',N' ورقة فقط لا غير',' sheets only ','Document Portfolio',1
EXEC [dbo].pInsTranslation 'Document_Portfolio_No_Of_Documents',N'عدد المستندات ',' No of documents ','Document Portfolio',1
EXEC [dbo].pInsTranslation 'Document_Portfolio_Appelant_Capacity',N'عن الطاعن بصفته','For the appellant in his capacity','Document Portfolio',1
EXEC [dbo].pInsTranslation 'Hearing_Description',N'ملخص المطلوب','Summary of What is Required','Hearings',1
EXEC [dbo].pInsTranslation 'Document_Source',N'مصدر المستند','Document Source','DMS',1


--- LLS Legal Principle 30-05-2024

EXEC [dbo].pInsTranslation 'Edit_Principle_Content',N'تحرير محتوى المبدأ','Edit Principle Content','LLS Legal Principle Content Edit Page',1

--- LLS Legal Principle 30-05-2024

-- LLS Literature 30-05-2024

EXEC [dbo].pInsTranslation 'Please_Add_Number_Pattern_Lookups_First',N'الرجاء إضافة عمليات البحث عن نمط الأرقام أولاً','Please Add Number Pattern Lookups First','Literature',1
--- LLS Legal Principle 30-05-2024

EXEC [dbo].pInsTranslation 'NumberOfPrincipleContents',N'عدد محتويات المبدأ','Number of Contents','Legal Principle',1


------------------- 11-6-2024 --------------------------
EXEC [dbo].pInsTranslation 'Authentication_Reason',N'سبب المصادقة','Authentication Reason','Consumer',1
EXEC [dbo].pInsTranslation 'Service_Description',N'وصف الخدمة','Service Description','Consumer',1
------------------- 11-6-2024 --------------------------

------------- Vendor and Contract Add fuel record
EXEC [dbo].pInsTranslation 'List_Car_Fuel_Record',N'قائمة سجل وقود السيارة','List Car Fuel Record','Vendor and Contrat',1
EXEC [dbo].pInsTranslation 'Add_Fuel_Record',N'إضافة سجل الوقود','Add Fuel Record','Vendor and Contrat',1
EXEC [dbo].pInsTranslation 'Fuel_Card_Number',N'رقم بطاقة الوقود','Fuel Card Number','Vendor and Contrat',1
EXEC [dbo].pInsTranslation 'Date_Of_Refuel',N'تاريخ التزود بالوقود','Date Of Refuel','Vendor and Contrat',1 
EXEC [dbo].pInsTranslation 'Total_Fuel_Price',N'إجمالي سعر الوقود','Total Fuel Price','Vendor and Contrat',1
EXEC [dbo].pInsTranslation 'View_Car_Fuel_Record',N'عرض سجل وقود السيارة','View Car Fuel Record','Vendor and Contrat',1
EXEC [dbo].pInsTranslation 'Card_Balance',N'رصيد البطاقة','Card Balance','Vendor and Contrat',1
EXEC [dbo].pInsTranslation 'Add_Car_Fuel_Record',N'إضافة سجل وقود السيارة','Add Car Fuel Record','Vendor and Contrat',1
EXEC [dbo].pInsTranslation 'Request_To_Refill',N'طلب إعادة التعبئة','Request to Refill','Vendor and Contrat',1
EXEC [dbo].pInsTranslation 'Request_To_Recharge',N'طلب إعادة الشحن','Request To Recharge','Vendor and Contrat',1
EXEC [dbo].pInsTranslation 'Recharge_Amount',N'مبلغ إعادة الشحن','Recharge Amount','Vendor and Contrat',1
EXEC [dbo].pInsTranslation 'Available_Balance',N'الرصيد المتوفر','Available Balance','Vendor and Contrat',1
EXEC [dbo].pInsTranslation 'Current_Balance',N'الرصيد الحالي','Current Balance','Vendor and Contrat',1
EXEC [dbo].pInsTranslation 'Fuel_Price',N'سعر الوقود','Fuel Price','Vendor and Contrat',1
EXEC [dbo].pInsTranslation 'Car_Fuel_Record_Not_Exits',N'سجل وقود السيارة غير موجود','Car Fuel Record Not Exits','Vendor and Contrat',1
EXEC [dbo].pInsTranslation 'Failed_To_Find_Car_Fuel_Record._Please_Try_Again',N'فشل في العثور على سجل وقود السيارة. حاول مرة اخرى','Failed To Find Car Fuel Record. Please Try Again','Vendor and Contrat',1
EXEC [dbo].pInsTranslation 'Confirm_Refill_Fuel_Card',N'تأكيد إعادة تعبئة بطاقة الوقود','Confirm Refill Fuel Card','Vendor and Contrat',1
EXEC [dbo].pInsTranslation 'Confirm_Add_Car_Fuel_Record',N'تأكيد إضافة سجل وقود السيارة','Confirm Add Car Fuel Record','Vendor and Contrat',1
EXEC [dbo].pInsTranslation 'Confirm_Update_Car_Fuel_Record',N'تأكيد تحديث سجل وقود السيارة','Confirm Update Car Fuel Record','Vendor and Contrat',1
EXEC [dbo].pInsTranslation 'Car_Fuel_Record_Successfully_Updated',N'تم تحديث سجل وقود السيارة بنجاح','Car Fuel Record Successfully Updated','Vendor and Contrat',1
EXEC [dbo].pInsTranslation 'Failed_To_Update_Car_Fuel_Record._Please_Try_Again',N'فشل في تحديث سجل وقود السيارة. حاول مرة اخرى','Failed To Update Car Fuel Record. Please Try Again','Vendor and Contrat',1
EXEC [dbo].pInsTranslation 'Car_Fuel_Record_Added_Successfully',N'تمت إضافة سجل وقود السيارة بنجاح','Car Fuel Record Added Successfully','Vendor and Contrat',1
EXEC [dbo].pInsTranslation 'Failed_To_Add_Car_Fuel_Record._Please_Try_Again',N'فشل في إضافة سجل وقود السيارة. حاول مرة اخرى','Failed To Add Car Fuel Record. Please Try Again','Vendor and Contrat',1
EXEC [dbo].pInsTranslation 'Edit_Car_Fuel_Record',N'تحرير سجل وقود السيارة','Edit Car Fuel Record','Vendor and Contrat',1

EXEC [dbo].pInsTranslation 'Update_Contract',N'تحديث العقد','Update Contract','Vendor Contract',1
EXEC [dbo].pInsTranslation 'NumberOfPrincipleContents',N'عدد محتويات المبدأ','Number of Contents','Legal Principle',1

/*<History Author='Ammaar Naveed' Date='25-06-2024'>Translation Employee Management</History>*/
EXEC [dbo].pInsTranslation 'User_Has_No_Tasks',N'المستخدم ليس لديه مهام جديدة','User has no tasks','Employee Deactivation',1
EXEC [dbo].pInsTranslation 'Last_Name_Criteria_Caption',N'"إضافة تلقائية للوصلة (-) لإسم عائلة الموظف إن كان يبدأ ب "ال"، "إبن" أو "بن','If employees last name is their family name then keep hyphen (-) between "Al", or "Bin", or "Ibn" otherwise remove hyphen (-)','Add Employee',1
EXEC [dbo].pInsTranslation 'Task_Delegation',N'تفويض المهمة','Task Delegation','Employee Deactivation',1
EXEC [dbo].pInsTranslation 'Employee_Confirm_Deactivate_Dialog_Title',N'تعطيل حساب موظف','Deactivate Employee Account','Employee Deactivation',1
EXEC [dbo].pInsTranslation 'Employee_Deactivate_Confirm_Dialog_Text',N'هل أنت متأكد من تعطيل حساب الموظف؟','Are you sure you want to deactivate this employee account?','Employee Deactivation',1
EXEC [dbo].pInsTranslation 'Task_Employee_Deactivate_Confirm_Dialog_Text',N'إن الموظف لا زال لديه مهام قيد الانتظار. هل أنت متأكد من تعطيل حسابه على أي حال؟','This employee has pending tasks. Are you sure you want to deactivate this employee anyway?','Employee Deactivation',1
EXEC [dbo].pInsTranslation 'Update_Contract',N'تحديث العقد','Update Contract','Vendor Contract',1

------- 25-06-2024 Legal Legislation start ---
EXEC [dbo].pInsTranslation 'Reference_Law',N'مرجع قانون','Reference Law','Legal Legislation List View Page',1
------- 25-06-2024 Legal Legislation end ---

-------------- Vendor And Contract (PlateNumber Loopkups Date : 27-june-024 ----------

EXEC [dbo].pInsTranslation 'List_Plate_Number',N'قائمة رقم اللوحة','List Plate Number','Plate Number Lookups',1
EXEC [dbo].pInsTranslation 'Car_Mileage',N'عدد الكيلومترات بالسيارة','Car Mileage','Plate Number Lookups',1
EXEC [dbo].pInsTranslation 'Add_Plate_Number',N'إضافة رقم اللوحة','Add Plate Number','Plate Number Lookups',1
EXEC [dbo].pInsTranslation 'Edit_Plate_Number',N'تعديل رقم اللوحة','Edit Plate Number','Plate Number Lookups',1
EXEC [dbo].pInsTranslation 'Plate_Number_List',N'لوحة_رقم_القائمة','Plate Number List','Plate Number Lookups',1
EXEC [dbo].pInsTranslation 'Are_You_Sure_Inactive_PlateNumber_Record',N'هل أنت متأكد من أن سجل رقم اللوحة غير نشط','Are you sure Inactive Plate Number','Plate Number Lookups',1
EXEC [dbo].pInsTranslation 'Are_You_Sure_Active_PlateNumber_Record',N'هل أنت متأكد من تسجيل رقم اللوحة النشط','Are you sure to active Plate Number','Plate Number Lookups',1
EXEC [dbo].pInsTranslation 'Are_You_Sure_To_Add_PlateNumber',N'هل أنت متأكد من إضافة رقم اللوحة',' Are you sure to add Plate Number','Plate Number Lookups',1
EXEC [dbo].pInsTranslation 'Are_You_Sure_To_Update_PlateNumber',N'هل أنت متأكد من تحديث سجل رقم اللوحة  ',' Are you sure to update Plate Number','Plate Number Lookups',1
EXEC [dbo].pInsTranslation 'PlateNumber_Record_Successfully_Updated',N'تم تحديث سجل رقم اللوحة بنجاح','Plate Number Successfully Updated','Plate Number Lookups',1
EXEC [dbo].pInsTranslation 'Failed_To_Update_PlateNumber_Lookup._Please_Try_Again',N'فشل في تحديث البحث عن رقم اللوحة. حاول مرة اخرى','Failed To Update Plate Number. Please Try Again','Plate Number Lookups',1
EXEC [dbo].pInsTranslation 'PlateNumber_Added_Successfully',N'تمت إضافة رقم اللوحة بنجاح','Plate Number added successfully','Plate Number Lookups',1
EXEC [dbo].pInsTranslation 'Failed_To_Add_PlateNumber_Lookup._Please_Try_Again',N'فشل في إضافة بحث عن رقم اللوحة. حاول مرة اخرى','Failed To Add Plate Number. Please Try Again','Plate Number Lookups',1
EXEC [dbo].pInsTranslation 'Mileage_Should_Be_Greater_Than_0',N'يجب أن تكون المسافة المقطوعة أكبر من 0.','Mileage should be greater than 0.','Plate Number Lookups',1
EXEC [dbo].pInsTranslation 'Mileage_Record_Added_successfully',N'تمت إضافة سجل الأميال بنجاح','Mileage Record Added successfully','Vendor and Contract',1
EXEC [dbo].pInsTranslation 'Task_Employee_Deactivate_Confirm_Dialog_Text',N'إن الموظف لا زال لديه مهام قيد الانتظار. هل أنت متأكد من تعطيل حسابه على أي حال؟','This employee has pending tasks. Are you sure you want to deactivate this employee anyway?','Employee Deactivation',1

/*<History Author='Ammaar Naveed' Date='27-06-2024'>UMS Lookups translations</History>*/
EXEC [dbo].pInsTranslation 'Designations',N'المسميات الوظيفية','Designations','UMS Lookups',1
EXEC [dbo].pInsTranslation 'Nationalities ',N'جنسيات','Nationalities','UMS Lookups',1
EXEC [dbo].pInsTranslation 'Grades',N'درجات','Grades','UMS Lookups',1
EXEC [dbo].pInsTranslation 'Contract_Types',N'أنواع العقود','Contract Types','UMS Lookups',1

/*<History Author='Ammaar Naveed' Date='02-07-2024'>Admin portal role assignment translations</History>*/
EXEC [dbo].pInsTranslation 'Assign_Role',N'تعيين الدور','Assign Role','Admin Portal UM',1
EXEC [dbo].pInsTranslation 'Role_Cannot_Be_Edited_Text',N'لا يمكنك تعديل هذا الدور، لدى الموظف مهام قيد التنفيذ.','You cannot edit this role, the employee has pending tasks.','Admin Portal UM',1
EXEC [dbo].pInsTranslation 'Confirm_Update_Role',N'هل أنت متأكد أنك تريد تحديث دور الموظف؟','Are you sure you want to update employee role?','Admin Portal UM',1
EXEC [dbo].pInsTranslation 'Confirm_Save_Role',N'هل أنت متأكد أنك تريد تعيين هذا الدور للموظف؟','Are you sure you want to assign this role to employee?','Admin Portal UM',1
EXEC [dbo].pInsTranslation 'Employee_Role_Added_Successfully',N'تم تعيين دور الموظف بنجاح','Employee role has been assigned successfully','Admin Portal UM',1
EXEC [dbo].pInsTranslation 'Employee_Role_Updated_Successfully',N'تم تحديث دور الموظف بنجاح','Employee role has been updated successfully','Admin Portal UM',1
EXEC [dbo].pInsTranslation 'Update_Employee_Role',N'تحديث دور الموظف','Update Employee Role','Admin Portal UM',1
EXEC [dbo].pInsTranslation 'Assign_Role_To_Employee',N'تعيين دور للموظف','Assign Role To Employee','Admin Portal UM',1
EXEC [dbo].pInsTranslation 'Assign_User_Role',N'تعيين دور المستخدم','Assign User Role','Admin Portal UM',1
EXEC [dbo].pInsTranslation 'User_Has_No_Tasks',N'المستخدم ليس لديه مهام جديدة','This user has no new tasks','Employee portal',1

----------------- Vendor and Contract 07-july-024
EXEC [dbo].pInsTranslation 'Contract_Cancelation_Reason',N' أضف سبب الإلغاء','Add Cancelation Reason','Vendor and Contrat',1
EXEC [dbo].pInsTranslation 'Are_You_Sure_To_Cancel_Contract',N'هل أنت متأكد من إلغاء العقد','Are You Sure To Cancel Contract','Vendor and Contrat',1
EXEC [dbo].pInsTranslation 'Contract_Canceled_Successfully',N'تم إلغاء العقد بنجاح','Contract Canceled Successfully','Vendor and Contrat',1
EXEC [dbo].pInsTranslation 'Failed_To_Cancel_The_Contract_Please_Try_Again',N'فشل إلغاء العقد، يرجى المحاولة مرة أخرى','Failed To Cancel The Contract. Please Try Again','Vendor and Contrat',1
EXEC [dbo].pInsTranslation 'Cancel_Contract',N'إلغاء العقد','Cancel Contract','Vendor and Contrat',1
EXEC [dbo].pInsTranslation 'Cancel_By',N'تم الإلغاء بواسطة','Canceled By','Vendor and Contrat',1
EXEC [dbo].pInsTranslation 'Cancel_Date',N'تاريخ الإلغاء','Canceled Date','Vendor and Contrat',1
EXEC [dbo].pInsTranslation 'Remaining_Balance',N'الرصيد المتبقي','Remaining Balance','Vendor and Contrat',1
EXEC [dbo].pInsTranslation 'Contract_Cancel_Reason',N'سبب إلغاء العقد','Contract Cancel Reason','Vendor and Contrat',1
EXEC [dbo].pInsTranslation 'Contract_Cancel_Reason',N'سبب إلغاء العقد','Contract Cancel Reason','Vendor and Contrat',1
----------
EXEC [dbo].pInsTranslation 'Alternative_Phone_No',N'رقم هاتف بديل','Alternate Phone No','Vendor and Contrat',1
EXEC [dbo].pInsTranslation 'Main_Phone_No',N'رقم الهاتف الرئيسي','Main Phone No','Vendor and Contrat',1
EXEC [dbo].pInsTranslation 'Fax_No',N'رقم الفاكس','Fax No','Vendor and Contrat',1
EXEC [dbo].pInsTranslation 'Commercial_Information',N'المعلومات التجارية','Commercial Information','Vendor and Contrat',1
EXEC [dbo].pInsTranslation 'Company_Classification',N'تصنيف الشركة','Company Classification','Vendor and Contrat',1
EXEC [dbo].pInsTranslation 'Commercial_Record_No',N'رقم السجل التجاري','Commercial Record No','Vendor and Contrat',1
EXEC [dbo].pInsTranslation 'Website',N'موقع إلكتروني','Website','Vendor and Contrat',1
EXEC [dbo].pInsTranslation 'Are_You_Sure_You_Want_to_Submit_Vendor_Detail',N'هل أنت متأكد أنك تريد تقديم تفاصيل البائع','Are you sure you want_to submit vendor detail','Vendor and Contrat',1




EXEC [dbo].pInsTranslation 'Contract_Types',N'أنواع العقود','Contract Types','UMS Lookups',1
EXEC [dbo].pInsTranslation 'Employee_Detail',N'تفاصيل الموظف','Employee Detail','General',1


----- LLS 18-07-2024 start
EXEC [dbo].pInsTranslation 'List_Vendors',N'قائمة البائعين','Vendor List','Vendor List Page',1

EXEC [dbo].pInsTranslation 'Add_Vendor_Detail',N'إضافة بائع','Add Vendor','Vendor List Page',1

EXEC [dbo].pInsTranslation 'Edit_Vendor_Detail',N'تحرير البائع','Edit Vendor','Vendor List Page',1

EXEC [dbo].pInsTranslation 'Company_Name_En',N'اسم الشركة (بالانجليزية)','Company Name (English)','Vendor List Page',1

EXEC [dbo].pInsTranslation 'Company_Name_Ar',N'اسم الشركة (عربي)','Company Name (Arabic)','Vendor List Page',1

EXEC [dbo].pInsTranslation 'Are_you_Sure_You_Want_to_Remove_Linked_Source_Record',N'هل أنت متأكد أنك تريد إزالة الرابط من المستند المصدر؟',
'Are you sure you want to remove the link from source document?','LLS Legal Principle Add Form Page',1

EXEC [dbo].pInsTranslation 'Legistion_Explanatory_Notes',N'المذكرة الايضاحية للتشريع','Legislation Explanatory Notes','Legislation List View Page',1

EXEC [dbo].pInsTranslation 'Legistion_Explantory_Notes',N'المذكرة الايضاحية للتشريع','Legislation Explanatory Notes','Legislation List View Page',1
EXEC [dbo].pInsTranslation 'Legistion_Note',N'ملاحظات','Legislation Note','Legislation List View Page',1
EXEC [dbo].pInsTranslation 'TagNo',N'رقم العلامة','Tag Number','Legal Literature Add Page',1
EXEC [dbo].pInsTranslation 'Add_Explanatory_Note_Attachment',N'إضافة مرفق المذكرة التوضيحية(pdf فقط)','Add explanatory note attachment (pdf only)','Legal Legislation Add Page',1
EXEC [dbo].pInsTranslation 'Reference_Law',N'مرجع قانون','Reference Law','Legal Legislation List View Page',1

----- LLS 18-07-2024 end
EXEC [dbo].pInsTranslation 'Permission_Type',N'نوع الإذن','Permission Type','Request Services',1 
EXEC [dbo].pInsTranslation 'Permission_Date',N'تاريخ الإذن','Permission Date','Request Services',1 
EXEC [dbo].pInsTranslation 'Total_Time_Duration',N'إجمالي المدة الزمنية','Total Time Duration','Request Services',1 
EXEC [dbo].pInsTranslation 'Hand_Over_To',N'تسليم','Hand Over To','Request Services',1 
EXEC [dbo].pInsTranslation 'Time_Required',N'الوقت اللازم','Time Required','Request Services',1 
EXEC [dbo].pInsTranslation 'Suggested_Appointment_Date',N'تاريخ الموعد المقترح','Suggested Appointment Date','Request Services',1 
EXEC [dbo].pInsTranslation 'Are_You_Sure_You_Want_to_Save_Draft',N'هل تريد حفظ كمسودة؟','Are you want to save as draft?','Request Services',1 
EXEC [dbo].pInsTranslation 'Add_Leave_Request',N'إضافة طلب إجازة','Add Leave Request','Request Services',1 
EXEC [dbo].pInsTranslation 'Time_Required',N'الوقت اللازم','Time Required','Request Services',1 
EXEC [dbo].pInsTranslation 'Request_Submitted_Successfully',N'تم تقديم الطلب بنجاح','Request Submitted Successfully','Request Services',1 
EXEC [dbo].pInsTranslation 'Draft_Saved_Successfully',N'تم حفظ المسودة بنجاح','Draft Saved Successfully','Request Services',1 
----- LLS 18-07-2024 end

----- LLS 18-07-2024 start
EXEC [dbo].pInsTranslation 'Add_Explanatory_Note_Attachment',N'إضافة مرفق المذكرة التوضيحية','Add explanatory note attachment','Legal Legislation Add Page',1
----- LLS 18-07-2024 end

/*<History Author='Ammaar Naveed' Date='10-07-2024'>Bell notification panel new translation</History>*/
EXEC [dbo].pInsTranslation 'View_All_Notification_Text',N'تود عرض جميع الزيارات؟ قم بزيارة','Want to view all notifications? Visit the','BellNotification',1

/*<History Author='Ammaar Naveed' Date='11-07-2024'>Translations Employee Portal</History>*/
EXEC [dbo].pInsTranslation 'Import_Successful',N'تم رفع مجموعة البيانات بنجاح','Bulk import has been completed successfully.','Bulk Import Employees',1
EXEC [dbo].pInsTranslation 'Required',N'مطلوب','Required','Whole application',1
EXEC [dbo].pInsTranslation 'Select_File',N'اختيار الملف','Select File','Bulkl Import',1
/*<History Author='Ammaar Naveed' Date='14-07-2024'>Translations Employee Portal</History>*/
EXEC [dbo].pInsTranslation 'Employee_Status_Updated_Success',N'تم تحديث حالة الموظف بنجاح','Employee status has been updated successfully.','Deactivate Employee',1

/*<History Author='Ammaar Naveed' Date='18-07-2024'>Translations Employee Portal</History>*/
UPDATE tTranslation SET Value_En='Error Log Details' WHERE TranslationKey='Error_logs' 

/*<History Author='Ammaar Naveed' Date='22-07-2024'>*/
EXEC [dbo].pInsTranslation 'Error_Logs',N'تفاصيل سجل الأخطاء','Error Log Details','Employee Portal',1
EXEC [dbo].pInsTranslation 'Claims_Updated',N'تم تحديث الإدعاء بنجاح','Claim has been updated successfully.','Admin Portal Claims',1

EXEC [dbo].pInsTranslation 'CAN',N'CAN.','CAN.',' History page',1
---23/07/2024
 EXEC [dbo].pInsTranslation 'Exemption_Time',N'وقت الإعفاء','Exemption Time','Request Services',1 
EXEC [dbo].pInsTranslation 'Exemption_Type',N'نوع الإعفاء','Exemption Type','Request Services',1 
EXEC [dbo].pInsTranslation 'Total_Duration',N'المدة الإجمالية','Total Duration','Request Services',1 
EXEC [dbo].pInsTranslation 'Start_Date',N'تاريخ البدء','Start Date','Request Services',1 
EXEC [dbo].pInsTranslation 'Days_Of_Absence',N'أيام الغياب','Days Of Absence','Request Services',1 
EXEC [dbo].pInsTranslation 'Leave_Balance',N'رصيد الاجازات','Leave Balance','Request Services',1 
EXEC [dbo].pInsTranslation 'Leave_Type',N'نوع الإجازة','Leave Type','Request Services',1 
EXEC [dbo].pInsTranslation 'Add_Reduce_Working_Hours_Request',N'إضافة طلب تقليل ساعات العمل','Add Reduce Working Hours Request','Request Services',1 
EXEC [dbo].pInsTranslation 'Add_Fingerprint_Exemption_Request',N'إضافة طلب إعفاء بصمة الإصبع','Add Fingerprint Exemption Request','Request Services',1 
EXEC [dbo].pInsTranslation 'Add_Permission_Request',N'إضافة طلب إذن','Add Permission Request','Request Services',1 
EXEC [dbo].pInsTranslation 'Add_Medical_Council_Request',N'طلب إضافة المجلس الطبي','Add Medical Council Request','Request Services',1 
EXEC [dbo].pInsTranslation 'Edit_Medical_Council_Request',N'تعديل طلب المجلس الطبي','Edit Medical Council Request','Request Services',1 
EXEC [dbo].pInsTranslation 'Edit_Permission_Request',N'تحرير طلب الإذن','Edit Permission Request','Request Services',1 
EXEC [dbo].pInsTranslation 'Edit_Fingerprint_Exemption_Request',N'تعديل طلب إعفاء بصمة الإصبع','Edit Fingerprint Exemption Request','Request Services',1 
EXEC [dbo].pInsTranslation 'Edit_Reduce_Working_Hours_Request',N'تعديل طلب تقليل ساعات العمل','Edit Reduce Working Hours Request','Request Services',1 
EXEC [dbo].pInsTranslation 'Edit_Leave_Request',N'تعديل طلب الإجازة','Edit Leave Request','Request Services',1 
EXEC [dbo].pInsTranslation 'Reduce_Working_Hours_Request_Detail',N'تفاصيل طلب تقليل ساعات العمل','Reduce Working Hours Request Detail','Request Services',1 
EXEC [dbo].pInsTranslation 'Leave_Request_Detail',N'ترك تفاصيل الطلب','Leave Request Detail','Request Services',1 
EXEC [dbo].pInsTranslation 'Fingerprint_Exemption_Request_Detail',N'تفاصيل طلب الإعفاء من بصمة الإصبع','Fingerprint Exemption Request Detail','Request Services',1 
EXEC [dbo].pInsTranslation 'Appointment_With_Medical_Council_Request_Detail',N'تفاصيل طلب موعد مع المجلس الطبي','Appointment With Medical Council Request Detail','Request Services',1 
EXEC [dbo].pInsTranslation 'Permission_Request_Detail',N'تفاصيل طلب الإذن','Permission Request Detail','Request Services',1 
EXEC [dbo].pInsTranslation 'Other_Attachment_Type',N'نوع المرفقات الأخرى','Other Attachment Type','General',1
EXEC [dbo].pInsTranslation 'Start_time_must_be_earlier_than_end_time',N'يجب أن يكون وقت البدء أقدم من وقت الانتهاء.','Start time must be earlier than end time.','General',1
EXEC [dbo].pInsTranslation 'End_time_must_be_later_than_start_time',N'يجب أن يكون وقت الانتهاء متأخرًا عن وقت البدء.','End time must be later than start time.','General',1
EXEC [dbo].pInsTranslation 'Must_be_at_least_a_4_hour_difference',N'المدة الإجمالية لا تزيد عن أربع ساعات.','Total time duration not more than four hours.','General',1
EXEC [dbo].pInsTranslation 'Claims_Updated',N'تم تحديث الإدعاء بنجاح','Claim has been updated successfully.','Admin Portal Claims',1
EXEC [dbo].pInsTranslation 'Country',N'بلد','Country','Add Employee Address',1

/*<History Author='Ammaar Naveed' Date='22-07-2024'>*/
EXEC [dbo].pInsTranslation 'Please_Assign_Group',N'بلد','Please assign a group to get Login Id','Add Employee Success',1
UPDATE tTranslation SET Value_En='The employee has been successfully created and the default password has also been generated which should be communicated along with the login id for login.' WHERE TranslationKey='Employee_Added_Success_AD'
UPDATE tTranslation SET Value_Ar=N'تم إنشاء الموظف بنجاح وتم أيضًا توليد كلمة المرور الافتراضية والتي يجب توصيلها مع معرف الدخول لتسجيل الدخول.' WHERE TranslationKey='Employee_Added_Success_AD'

/*<History Author='Ammaar Naveed' Date='28-07-2024'>*/
EXEC [dbo].pInsTranslation 'Disabled_Assigned_Users',N'Disabled users have tasks assigned, they cannot be removed from the list. To remove the desired users, please clear the pending tasks first.','Disabled users have tasks assigned, they cannot be removed from the list. To remove the desired users, please clear the pending tasks first.','Edit Group Screen',1

---- 31-july-2024
EXEC [dbo].pInsTranslation 'Something_Went_Wrong_With_Notifications',N'Something went wrong with Notifications, please try again later','Something went wrong with notifications, Please try again later','Notificaiton bell',1
EXEC [dbo].pInsTranslation 'Notification_Updation',N'Unable to process notifications. Please try again later','Unable to process notifications. Please try again later','Notificaiton bell',1

/*<History Author='Ammaar Naveed' Date='31-07-2024'>*/
EXEC [dbo].pInsTranslation 'Please_Assign_Group',N'يرجى تعيين الموظف لمجموعة المستخدمين للحصول على اسم المستخدم','Please assign the employee to a users groups to get the login Id','Add New Employee',1
EXEC [dbo].pInsTranslation 'View_Group_Details',N'عرض تفاصيل المجموعة','View Group Details','View Group',1
EXEC [dbo].pInsTranslation 'Disabled_Assigned_Users',N' الموظفين الذين تم تعطيلهم لديهم مهام قيد العمل، لا يمكنك ازالة الموظف من القائمة الا بعد الانتهاء من جميع المهام قيد العمل لديه',' Disabled employees had in progress tasks to remove an employee from the list. You need to clear all the in progress tasks for him','Add/Edit Group',1
EXEC [dbo].pInsTranslation 'Cannot_Update_Disabled_Users_Role',N'الموظفين الذين تم تعطيلهم لديهم مهام قيد العمل، لتعديل الدور يجب الانتهاء من جميع المهام قيد العمل لدى الموظف','Disabled employees had in progress tasks to update the role. You need to clear all the in progress tasks of that employee','Add/Edit Role',1
EXEC [dbo].pInsTranslation 'Default_Correspondence_Receiver_Updated',N'تم تعديل مستلم المراسلات الافتراضي','The default correspondence receiver has been updated successfully','Assign Role Screen',1
EXEC [dbo].pInsTranslation 'Update_Deault_Receiver',N'تعديل مستلم المراسلات الافتراضي','Update Default Correspondence Receiver','Assign Role Screen',1
EXEC [dbo].pInsTranslation 'Update_Default_Receiver_Status',N'هل انت متأكد انك تريد تغيير مستلم المراسلات الافتراضي للمستخدم تم اختياره؟',' Are you sure you want to update the default correspondence receiver to the selected user?','Assign Role Screen',1
EXEC [dbo].pInsTranslation 'Absent_Till_Date',N'غائب لتاريخ','Absent TIll Date','Assign To Lawyer Screen',1

/*<History Author='Ammaar Naveed' Date='01-08-2024'>*/
EXEC [dbo].pInsTranslation 'Employee_Status_Updated_Success',N'تم تحديث حالة الموظف بنجاح','Employee status has been updated successfully.','Activate/Deactivate Employee',1
EXEC [dbo].pInsTranslation 'Add',N'إضافة','Add','Popups',1
EXEC [dbo].pInsTranslation 'Proceed',N'كمل','Proceed','Add User Type',1

/*<History Author='Ammaar Naveed' Date='06-08-2024'>*/
EXEC [dbo].pInsTranslation 'Digital_Signature',N'التوقيع الإلكتروني','Digital Signature','Digital Signature',1
EXEC [dbo].pInsTranslation 'Action',N'الاجراء','Action','Digital Signature',1
EXEC [dbo].pInsTranslation 'Proceed',N'متابعة','Proceed','Digital Signature',1

EXEC [dbo].pInsTranslation 'Must_Eight_Characters',N'يجب أن يكون رقم الاتصال 8 أحرف فقط','Contact number should be 8 characters long','Contact Managment',1

EXEC [dbo].pInsTranslation 'Must_Eight_Characters',N'رقم التواصل يجب ان يكون ٨ ارقام','The contact number must be 8 digits','Contact Managment',1

/*<History Author='Ammaar Naveed' Date='11-08-2024'>Assign User Claims Translations</History>*/
EXEC [dbo].pInsTranslation 'Assign_User_Claims',N'تعيين صفحات المستخدمين','Assign Digital Signature Claims','Assign User Claims',1
EXEC [dbo].pInsTranslation 'Assign_Claims_To_Users',N'تعيين الصفحات للمستخدمين','Assign Digital Signature to Users','Assign User Claims',1
EXEC [dbo].pInsTranslation 'User_Claims',N'صفحات المستخدمين','Digital Signature Claims','Assign User Claims',1
EXEC [dbo].pInsTranslation 'Confirm_Add_Claims',N'هل أنت متأكد أنك تريد إضافة الصفحات للمستخدمين المحددين؟','Are you sure you want to add the digital signature for selected users?','Assign User Claims',1
EXEC [dbo].pInsTranslation 'User_Claims_Added_Successfully',N'تم إضافة الصفحة بنجاح','Digital signature claims have been added successfully','Assign User Claims',1
EXEC [dbo].pInsTranslation 'Assign_User_Role_And_Claims',N'تعيين دور وصلاحيات المستخدم','Assign User Role & Claims','Assign User Claims',1
EXEC [dbo].pInsTranslation 'Send_For_Approval',N'إرسال للموافقة','Send For Approval','Meeting Module',1
EXEC [dbo].pInsTranslation 'Sure_Send_For_Approval',N'هل تريد التقديم للموافقة؟','Are you want to submit for approval?','General',1
EXEC [dbo].pInsTranslation 'Edit_Meeting',N'تحرير الاجتماع','Edit Meeting','Meeting Module',1 

/*<History Author='Ammaar Naveed' Date='19-08-2024'>Assign role and claim screen new translations</History>*/
EXEC [dbo].pInsTranslation 'Confirm_Save_Role_Bulk_Users',N'هل أنت متأكد أنك تريد تعيين هذا الدور للمستخدمين تم اختيارهم؟','Are you sure you want to assign this role to selected users?','Assign bulk user role',1
EXEC [dbo].pInsTranslation 'Assign_Digital_Signature',N'تعيين التوقيع الإلكتروني','Assign Digital Signature','Assign user claims',1
EXEC [dbo].pInsTranslation 'Digital_Signature_Methods',N'طرق التوقيع الإلكتروني','Digital Signature Methods','Assign user claims & Role screen',1
EXEC [dbo].pInsTranslation 'Filter_Users',N'المستخدمين','Filter Users','Assign user claims & Role screen',1
EXEC [dbo].pInsTranslation 'Assigned_Digital_Signature_Methods',N'تعيين طرق التوقيع الإلكتروني','Assigned Digital Signature Methods','Assign user claims & Role screen',1
EXEC [dbo].pInsTranslation 'User_Role_And_Digital_Signature',N'طرق دور المستخدم والتوقيع الرقمي','User Role & Digital Signature','Sidemenu',1
EXEC [dbo].pInsTranslation 'Edit_Meeting',N'تحرير الاجتماع','Edit Meeting','Meeting Module',1 


/*<History Author='Muhammad Ismail' Date='21-08-2024'>Leave and attendance Translations</History>*/
 EXEC [dbo].pInsTranslation 'Need_Mofification',N'بحاجة للتحريف','Need Mofification','Leave and Attendance',1
 EXEC [dbo].pInsTranslation 'Reason_For_Need_Modification',N'سبب الحاجة إلى التعديل','Reason For Need Modification','Leave and Attendance',1
 EXEC [dbo].pInsTranslation 'Sure_Delete',N'هل أنت متأكد أنك تريد الحذف؟','Are you sure you want to delete?','G2G Case Management',1
 
/*<History Author='Muhammad Ismail' Date='22-08-2024'>Leave and attendance Translations</History>*/
 EXEC [dbo].pInsTranslation 'Received_Service_Requests',N'طلبات الخدمة المستلمة','Received Service Requests','Leave Attendance',1
 EXEC [dbo].pInsTranslation 'Submitted_Service_Requests',N'طلبات الخدمة المقدمة','Submitted Service Requests','Leave Attendance',1
  ----------- Ttranslation ------------
 
EXEC [dbo].pInsTranslation 'Add_Leave_Request',N'إضافة طلب إجازة','Add Leave Request','Request Services',1
EXEC [dbo].pInsTranslation 'Service_Request_Number',N'رقم طلب الخدمة','Service Request Number','Service Request',1
EXEC [dbo].pInsTranslation 'Sector_Sent_By',N'القطاع (مرسل بواسطة)','Sector (Sent By)','Service Request',1
EXEC [dbo].pInsTranslation 'Service_Request_Rejected',N'تم رفض طلب الخدمة','Service Request Rejected','Service Request',1
EXEC [dbo].pInsTranslation 'Reason_For_Need_Modification',N'سبب الحاجة إلى التعديل','Reason For Need Modification','Leave and Attendance',1
EXEC [dbo].pInsTranslation 'Added_By',N'تمت الإضافة بواسطة','Added By','Leave and Attendance',1
EXEC [dbo].pInsTranslation 'Request_Submitted_Successfully',N'تم تقديم الطلب بنجاح','Request Submitted Successfully','Service Request',1
EXEC [dbo].pInsTranslation 'Hand_Over_To',N'تسليم','Hand Over To','Request Services',1
EXEC [dbo].pInsTranslation 'Draft_Saved_Successfully',N'تم حفظ المسودة بنجاح','Draft Saved Successfully','Request Services',1
EXEC [dbo].pInsTranslation 'Are_You_Sure_You_Want_to_Save_Draft',N'هل تريد حفظ كمسودة؟','Are you want to save as draft?','Request Services',1
EXEC [dbo].pInsTranslation 'Need_Modification',N'بحاجة إلى تعديل','Need Modification','Services Request',1
EXEC [dbo].pInsTranslation 'NeedModification',N'بحاجة إلى تعديل','Need Modification','Services Request',1
EXEC [dbo].pInsTranslation 'Need_Modification_Remarks_Submitted_Successfully',N'تحتاج إلى ملاحظات التعديل المقدمة بنجاح','Need Modification Remarks Submitted Successfully','Services Request',1
EXEC [dbo].pInsTranslation 'Users_To_Sent_Task_Not_Exists_Contact_Administrator',N'المستخدمون الذين يرسلون المهمة غير موجودة، اتصل بالمسؤول','Users To Send Task Not Exists Contact Administrator','Services Request',1
EXEC [dbo].pInsTranslation 'Upload_Decision_Form',N'تحميل نموذج القرار','Upload Decision Form','Services Request',1
EXEC [dbo].pInsTranslation 'Received_Service_Requests',N'طلبات الخدمة المستلمة','Received Service Requests','Leave Attendance',1
EXEC [dbo].pInsTranslation 'ReceivSubmitted_Service_Requestsed_Service_Requests',N'طلبات الخدمة المقدمة','Submitted Service Requests','Leave Attendance',1
EXEC [dbo].pInsTranslation 'Payment_Applicable',N'ينطبق الدفع','Payment Applicable','Leave Attendance',1
EXEC [dbo].pInsTranslation 'Total_Time_Duration',N'إجمالي المدة الزمنية','Total Time Duration','Request Services',1
EXEC [dbo].pInsTranslation 'Suggested_Appointment_Date',N'تاريخ الموعد المقترح','Suggested Appointment Date','Request Services',1
EXEC [dbo].pInsTranslation 'Document_Uploaded_Successfully',N'تم تحميل المستند بنجاح','Document Uploaded Successfully','Request Services',1

------------------- 24-08-024 ---------
EXEC [dbo].pInsTranslation 'Weekends_Holidays',N'عطلات نهاية الأسبوع','Weekends Holidays','Request Services',1
EXEC [dbo].pInsTranslation 'Reject_Leave_Request',N'رفض طلب الإجازة','Reject Leave Request','Request Services',1
EXEC [dbo].pInsTranslation 'Permission_Date',N'تاريخ الإذن','Permission Date','Request Services',1
EXEC [dbo].pInsTranslation 'Permission_Type',N'نوع الإذن','Permission Type','Request Services',1
EXEC [dbo].pInsTranslation 'Submitted_Service_Requests',N'طلبات الخدمة المقدمة','Submitted Service Requests','Request Services',1
--- 26/august/2024
EXEC [dbo].pInsTranslation 'Unauthorized_Digital_Signature_Signing',N'ليس لديك صلاحية للتوقيع الإلكتروني','You do not have any permission to sign digitally','Digital Signature',1
EXEC [dbo].pInsTranslation 'Document_type_Added_Successfully',N'تمت إضافة نوع المستند بنجاح','Document type added successfully','DMS ENUM FIX lookup ADMIN',1
EXEC [dbo].pInsTranslation 'Document_type_Updated_Successfully',N'تم تحديث نوع المستند بنجاح','Document type updated successfully','DMS ENUM FIX lookup ADMIN',1

 EXEC [dbo].pInsTranslation 'Logout',N'تسجيل الخروج','Logout','General',1
EXEC [dbo].pInsTranslation 'Attendee_Decision',N'قرار حاضري الاجتماع','Attendees Decision','General',1
EXEC [dbo].pInsTranslation 'Please_Select_File',N'يرجى اختيار الملف لرفعه','Please select a file to upload','General',1
EXEC [dbo].pInsTranslation 'Submit_MOM',N'إرسال محضر الاجتماع','Submit Meeting Minutes','General',1
EXEC [dbo].pInsTranslation 'View_Item_History',N'عرض التغييرات','View History','General',1
EXEC [dbo].pInsTranslation 'View_TaskDetails',N'عرض تفاصيل المهمة','View Task Details','General',1
EXEC [dbo].pInsTranslation 'Changes_saved_successfully',N'تم حفظ التغييرات بنجاح.','Changes saved successfully.','General',1
EXEC [dbo].pInsTranslation 'Copy_Sent',N'تم ارسال نسخة من الطلب بنجاح.','Copy sent successfully.','General',1
EXEC [dbo].pInsTranslation 'Duration',N'المدة (أيام)','Duration (Days)','General',1
------
EXEC [dbo].pInsTranslation 'Default_Correspondence_Receiver',N'مستلم المراسلات الافتراضي','Default Correspondence Receiver','General',1
EXEC [dbo].pInsTranslation 'First_Reminder_Duration_Should_be_Less_Than_SLA_Interval',N'مدة أول تذكير يجب أن تكون أقل من المدة الزمنية المطلوبة','First Reminder Duration Should be Less than SLA Interval','General',1
EXEC [dbo].pInsTranslation 'Go_Back',N'عودة','Back','General',1
EXEC [dbo].pInsTranslation 'List_MOM_Documents',N'قائمة مستندات محضر الاجتماع','Meeting Minutes Documents List','General',1
EXEC [dbo].pInsTranslation 'Mom_Decision',N'قرار محضر الاجتماع','Meeting Minutes Decision','General',1
EXEC [dbo].pInsTranslation 'MOM_Draft',N'مسودة محضر الاجتماع','Meeting Minutes Draft','General',1
EXEC [dbo].pInsTranslation 'Please_Select_File',N'يرجى اختيار الملف لرفعه','Please select a file to upload','General',1
EXEC [dbo].pInsTranslation 'Reference_Screen',N'Reference Screen','الشاشة المرجعية','General',1
EXEC [dbo].pInsTranslation 'ReminderType',N'نوع التذكير','Reminder Type','General',1
EXEC [dbo].pInsTranslation 'SLA_Interval_Day',N'المدة الزمنية (أيام)','SLA Interval (Days)','General',1
EXEC [dbo].pInsTranslation 'Submit_MOM_to_Ge',N'إرسال محضر الاجتماع للجهة الحكومية','Submit Meeting Minutes to GE','General',1
EXEC [dbo].pInsTranslation 'Sure_Held_Meeting',N'هل أنت متأكد أنك تريد عقد الاجتماع؟','Are you sure you want to held the meeting?','General',1
EXEC [dbo].pInsTranslation 'Time_Interval_History',N'التغييرات على المدة الزمنية','Time Interval History','General',1
EXEC [dbo].pInsTranslation 'Translation_Added',N'تم إضافة الترجمة','Translation Added ','General',1
EXEC [dbo].pInsTranslation 'Translation_Id',N'معرف الترجمة','Translation Id','General',1
EXEC [dbo].pInsTranslation 'Translation_Key',N'مفتاح الترجمة','Translation Key','General',1
EXEC [dbo].pInsTranslation 'Translation_Type',N'نوع الترجمة','Translation Type','General',1
EXEC [dbo].pInsTranslation 'Translation_Updated',N'تم تعديل الترجمة','Translation Updated','General',1
EXEC [dbo].pInsTranslation 'Update_Translation',N'تعديل الترجمة','Edit Translation','General',1
EXEC [dbo].pInsTranslation 'Updated_Date',N'تاريخ التعديل','Updated Date','General',1
EXEC [dbo].pInsTranslation 'Value_Ar',N'الاسم (عربي)','Name (Arabic)','General',1
EXEC [dbo].pInsTranslation 'Value_En',N'الاسم (انجليزي)','Name (English)','General',1
EXEC [dbo].pInsTranslation 'Lawyer_Assigned',N'تم تعيين المحامي','Lawyer has been Assigned','Assign Lawyer To Court',1
EXEC [dbo].pInsTranslation 'Create_Execution_Request',N'إنشاء طلب التنفيذ','Create Execution Request','MOJ',1
EXEC [dbo].pInsTranslation 'CaseNumber_Exists',N'رقم الحالة موجود بالفعل','Case number already exists','CMS',1
EXEC [dbo].pInsTranslation 'Sure_Submit_Case',N'هل أنت متأكد أنك تريد تقديم تفاصيل الحالة؟','Are you sure you want to submit case details?','Tasks List Screen',1
EXEC [dbo].pInsTranslation 'Other_Cases',N'حالات أخرى','Other Cases','Create Registered page ',1
EXEC [dbo].pInsTranslation 'Upload_Claim_Statement',N'يرجى تحميل بيان المطالبة.','Please upload claim statement.','MOJ Registration',1
EXEC [dbo].pInsTranslation 'Case_Open_Date',N'تاريخ فتح القضية','Case Open Date','Register Case page',1
EXEC [dbo].pInsTranslation 'Must_Twelve_Characters',N'يجب أن تكون قيمة ١٢ رقمًا.',' Value must be a 12 digits.','General',1
EXEC [dbo].pInsTranslation 'View_Correspondence_Detail',N'عرض تفاصيل المراسلات','View Correspondence Detail','Detail Case File',1
EXEC [dbo].pInsTranslation 'View_Detail',N'عرض التفاصيل','View Detail','General',1
EXEC [dbo].pInsTranslation 'No_File_Selected',N'لم يتم تحديد ملف','No File Selected','Case Management',1
EXEC [dbo].pInsTranslation 'Mark_As_Important',N'وضع علامة على أنها مهمة','Mark As Important','General',1
EXEC [dbo].pInsTranslation 'Mark_As_Un_Important',N'وضع علامة كغير مهم','Mark As UnImportant','General',1
EXEC [dbo].pInsTranslation 'CRN',N'رقم السحل التجاري','CRN','Case File',1
EXEC [dbo].pInsTranslation 'CivilId/CRN',N'رقم البطاقة المدنية/رقم السجل التجاري','CivilId/CRN','General',1
EXEC [dbo].pInsTranslation 'GE_Attendees',N'الحاضرين من الجهة الحكومية','Selected GE Attendees','General',1
-------- 27-08-024 ----------------
EXEC [dbo].pInsTranslation 'Review_Casual_Leave_Request',N'مراجعة طلب الإجازة غير الرسمية','Review Casual Leave Request','Request Services',1
EXEC [dbo].pInsTranslation 'Review_Annual_Leave_Request',N'مراجعة طلب الإجازة السنوية','Review Annual Leave Request','Request Services',1
EXEC [dbo].pInsTranslation 'Review_Special_Leave_Request',N'مراجعة طلب الإجازة الخاصة','Review Special Leave Request','Request Services',1
EXEC [dbo].pInsTranslation 'Review_Maternity_Leave_Request',N'مراجعة طلب إجازة الأمومة','Review Maternity Leave Request','Request Services',1
EXEC [dbo].pInsTranslation 'Review_Sick_Leave_Request',N'مراجعة طلب الإجازة المرضية','Review Sick Leave Request','Request Services',1
EXEC [dbo].pInsTranslation 'Review_Reduce_Working_Hours_Request',N'مراجعة طلب تقليل ساعات العمل','Review Reduce Working Hours Request','Request Services',1
EXEC [dbo].pInsTranslation 'Review_Fingerprint_Exemption_Request',N'مراجعة طلب الإعفاء من بصمة الإصبع','Review Fingerprint Exemption Request','Request Services',1
EXEC [dbo].pInsTranslation 'Review_Permission_Request',N'مراجعة طلب الإذن','Review Permission Request','Request Services',1
EXEC [dbo].pInsTranslation 'Review_Medical_Council_Request',N'مراجعة طلب المجلس الطبي','Review Medical Council Request','Request Services',1


---29-aug-2024
EXEC [dbo].pInsTranslation 'Document_Status_History',N'سجل حالة المستند','Document Status History','DMS',1
EXEC [dbo].pInsTranslation 'Service_Name',N'اسم الخدمة','Service Name','DMS',1
EXEC [dbo].pInsTranslation 'Service',N'خدمة','Service','DMS',1
EXEC [dbo].pInsTranslation 'Send_For_Signing',N'Send For Signing','Send For Signing','DMS',1
EXEC [dbo].pInsTranslation 'Is_Digitally_Sign',N'Is Digitally Sign','Is Digitally Sign','DMS',1

-----
EXEC [dbo].pInsTranslation 'Reminder_type',N'نوع التذكير','Reminder Type','General',1
EXEC [dbo].pInsTranslation 'First_Reminder_Duration',N'مدة التذكير الأول','First Reminder Duration','General',1
EXEC [dbo].pInsTranslation 'Second_Reminder_Duration',N'مدة التذكير الثاني','Second Reminder Duration','General',1
EXEC [dbo].pInsTranslation 'Third_Reminder_Duration',N'مدة التذكير الثالث','Third Reminder Duration','General',1
EXEC [dbo].pInsTranslation 'Execution_Time',N'وقت التنفيذ','Execution Time','General',1
EXEC [dbo].pInsTranslation 'Last_Modified',N'آخر تعديل','Last Modified','General',1
EXEC [dbo].pInsTranslation 'SLA_Interval',N'المدد الزمنية','SLA Interval','General',1
EXEC [dbo].pInsTranslation 'Activity_Name',N'اسم الاجراء','Activity Name','General',1
EXEC [dbo].pInsTranslation 'Execution_Status',N'حالة ملف التنفيذ','Execution Status','General',1
EXEC [dbo].pInsTranslation 'GEs',N'الجهات الحكومية','Government Entities','General',1

--- 03-sep-2024
EXEC [dbo].pInsTranslation 'Signing_Status_History',N'سجل حالة التوقيع','Signing Status History','DMS',1
EXEC [dbo].pInsTranslation 'Draft_Version_History',N'محفوظات عمل إصدار المسودة','Draft Version Action History','General',1
EXEC [dbo].pInsTranslation 'Case_Draft_Created_Successfully',N'تم إرسال مسودة مستند الحالة بنجاح.','Case draft document has been submitted successfully.','General',1
EXEC [dbo].pInsTranslation 'Font',N'الخط','Font','General',1
EXEC [dbo].pInsTranslation 'Format_Block',N'كتلة التنسيق','Format Block','General',1
EXEC [dbo].pInsTranslation 'Font_Size',N'حجم الخط','Font Size','General',1
EXEC [dbo].pInsTranslation 'CivilId_CRN',N'رقم البطاقة المدنية/رقم السجل التجاري','Civil ID/CRN	','General',1

EXEC [dbo].pInsTranslation 'Sign_by_Civil_ID',N'طريق البطاقة المدنية التوقيع عن','Sign By Civil ID','DS',1
EXEC [dbo].pInsTranslation 'Sign_by_Mobile_Auth',N'الهاتف التوقيع عن طريق','Sign By Mobile Auth','DS',1
EXEC [dbo].pInsTranslation 'Sign_by_KMID',N'التوقيع عن طريق هويتي','Sign By KMID','DS',1
EXEC [dbo].pInsTranslation 'Allow_DS',N'Allow Digital Signature',' Allow Digital Signature','DS',1


------------- 10/09/2024---------------
EXEC [dbo].pInsTranslation 'Response',N'إجابة','Response','General',1
EXEC [dbo].pInsTranslation 'Sector_Received_By',N'تم الاستلام بواسطة','Received By','General',1
EXEC [dbo].pInsTranslation 'MojRolls_Requests',N'طلبات الرول من وزارة العدل','MojRolls Requests','General',1
EXEC [dbo].pInsTranslation 'Must_nine_Characters',N'يجب أن يكون رقم الحالة ٩ أرقام','Case Number Must be 9 digits','General',1
EXEC [dbo].pInsTranslation 'Total_Amount',N'المجموع','Total','General',1
EXEC [dbo].pInsTranslation 'View_all_notification_text',N'تود عرض جميع الزيارات؟ قم بزيارة','Want to view all notifications? Visit the','General',1
EXEC [dbo].pInsTranslation 'Notification_Page',N'صفحة الإشعارات','Notification Page','General',1
EXEC [dbo].pInsTranslation 'Roll_type',N'نوع الرول','Roll Type','General',1
EXEC [dbo].pInsTranslation 'CaseStudy_Status',N'حالة دراسة القضية','Case Study Status','General',1
EXEC [dbo].pInsTranslation 'Session_Date',N'تاريخ الجلسة','Hearing Date','General',1
EXEC [dbo].pInsTranslation 'Case_Detail',N'تفاصيل القضية','Case Details','General',1
------------- 10/09/2024---------------

EXEC [dbo].pInsTranslation 'Add_Author_Details',N'أضف تفاصيل المؤلف','Add Author Details','General',1
EXEC [dbo].pInsTranslation 'Fill_Required_Field',N'أضف تفاصيل المؤلف','Fill Required Field','General',1 
EXEC [dbo].pInsTranslation 'Index_Details',N'رقم/اسم الفهرس','Index Number/Name','General',1
EXEC [dbo].pInsTranslation 'Edition_Year',N'سنة الطبعة','Edition Year','General',1

---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------- LMS STOCKTAKING START -----------------------------------------------------------------------------------------------------
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


EXEC [dbo].pInsTranslation 'Create_StockTaking_Report',N'إنشاء تقرير جرد الكتب','Create Books Stocktaking Report','Add Stock Taking',1
EXEC [dbo].pInsTranslation 'StockTaking_List',N'قائمة جرد الكتب','Books Stocktaking List','Add Stock Taking',1
EXEC [dbo].pInsTranslation 'StockTaking_Date',N'تاريخ الجرد','StockTaking Date','Add Stock Taking',1
EXEC [dbo].pInsTranslation 'Total_No_Of_Books',N'إجمالي الكتب','Total No of Books','Add Stock Taking',1
EXEC [dbo].pInsTranslation 'Generate_Book_List',N'إنشاء قائمة الكتب','Generate Books List','Add Stock Taking',1
EXEC [dbo].pInsTranslation 'Insert_Remarks',N'إضافة ملاحظات','Add Remarks','Add Stock Taking',1
EXEC [dbo].pInsTranslation 'RFID_Value',N'RFID رقم','RFID Value','Add Stock Taking',1
EXEC [dbo].pInsTranslation 'Total_Copies_Borrowed',N'تم استعارته','Borrowed','Add Stock Taking',1
EXEC [dbo].pInsTranslation 'Total_Copies_Not_Borrowed',N'لم يتم استعارته','Not Borrowed','Add Stock Taking',1
EXEC [dbo].pInsTranslation 'Excess/Shortage',N'نقص/فائض','Excess/Shortage','Add Stock Taking',1
EXEC [dbo].pInsTranslation 'Shortage',N'نقص','Shortage','Add Stock Taking',1
EXEC [dbo].pInsTranslation 'Import_List_And_Compare',N'رفع قائمة الجرد وعمل مقارنة','Import Stocktaking List and Compare','Add Stock Taking',1
EXEC [dbo].pInsTranslation 'Print_Report',N'طباعة التقرير','Print Report','Add Stock Taking',1
EXEC [dbo].pInsTranslation 'Import_And_Compare',N'رفع القائمة وعمل مقارنة','Import And Compare','Add Stock Taking',1
EXEC [dbo].pInsTranslation 'StockTaking_Report_Successfully_Saved',N'تم حفظ تقرير جرد الكتب بنجاح','Books Stocktaking Report Successfully Saved','Add Stock Taking',1
EXEC [dbo].pInsTranslation 'Are_you_sure_you_want_to_save_the_report',N'هل أنت متأكد أنك تريد حفظ تقرير جرد الكتب؟','Are you sure you want to save the report?','Add Stock Taking',1
EXEC [dbo].pInsTranslation 'Employee_Who_Performed_StockTaking',N'الموظفين الذين قاموا بالجرد','Employees who Performed Stocktaking','Add Stock Taking',1
EXEC [dbo].pInsTranslation 'Totals_Book_Counted',N'إجمالي الكتب التي تم إحصاؤها','Total Books Counted','Add Stock Taking',1
EXEC [dbo].pInsTranslation 'Import_File',N'رفع الملف','Import File','Add Stock Taking',1
EXEC [dbo].pInsTranslation 'Compare_Stocktaking',N'عمل مقارنة','Compare','Add Stock Taking',1
EXEC [dbo].pInsTranslation 'Please_Select_Correct_File',N'يرجى اختيار الملف الصحيح','Please Select the Correct File','Add Stock Taking',1
EXEC [dbo].pInsTranslation 'Stock_Taking_Report',N'تقرير جرد الكتب','Books Stocktaking Report','Stock Taking List',1
EXEC [dbo].pInsTranslation 'Stock_Taking_Details',N'تفاصيل جرد الكتب','Books Stocktaking Details','Stock Taking Details',1
EXEC [dbo].pInsTranslation 'Approved_StockTaking_Report',N'هل أنت متأكد أنك تريد اعتماد تقرير الجرد؟','Are you sure you want to approve the stocktaking report?','Stock Taking List',1
EXEC [dbo].pInsTranslation 'StockTaking_Report_has_been_Approved_Successfully',N'تم اعتماد تقرير الجرد بنجاح','Stocktaking report has been approved successfully','Stock Taking List',1
EXEC [dbo].pInsTranslation 'Approve_Report',N'اعتماد الجرد','Approve Stocktaking','Add StockTaking',1
EXEC [dbo].pInsTranslation 'Report_Number',N'رقم التقرير','Report Number','Detail StockTaking',1
EXEC [dbo].pInsTranslation 'StockTaking_Performers',N'القائمين على الجرد','Stocktaking Performers','Detail StockTaking',1
EXEC [dbo].pInsTranslation 'Added_By',N'تم إضافة من قبل','Added By','Detail StockTaking',1
EXEC [dbo].pInsTranslation 'StockTaking_Date_From',N'من تاريخ الجرد','StockTaking Date From','List StockTaking',1
EXEC [dbo].pInsTranslation 'StockTaking_Date_To',N'إالى تاريخ الجرد','StockTaking Date To','List StockTaking',1
EXEC [dbo].pInsTranslation 'Copies_Borrowed',N'تم استعارته','Borrowed','Detail Stock Taking',1
EXEC [dbo].pInsTranslation 'Copies_Not_Borrowed',N'لم يتم استعارته','Not Borrowed','Detail Stock Taking',1
EXEC [dbo].pInsTranslation 'Book_Name',N'اسم الكتاب','Book Name','Detail Stock Taking',1

EXEC [dbo].pInsTranslation 'Select_File',N'اختيار الملف','Select File','Import Stocktaking',1
EXEC [dbo].pInsTranslation 'Author',N'مؤلف','Author','Import Stocktaking',1
EXEC [dbo].pInsTranslation 'Books_StockTaking_Details',N'تفاصيل جرد الكتب','Books StockTaking Details','Detail Stocktaking',1
EXEC [dbo].pInsTranslation 'Books_StockTaking_Reports',N'تقارير جرد الكتب','Books StockTaking Reports','Detail Stocktaking',1

EXEC [dbo].pInsTranslation 'Excess_Shortage',N'نقص/فائض','Excess/Shortage','Add Stock Taking',1
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------- LMS STOCKTAKING END -----------------------------------------------------------------------------------------------------
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
 
 EXEC [dbo].pInsTranslation 'Undo',N'تراجع','Undo','General',1
EXEC [dbo].pInsTranslation 'Redo',N'إعادة','Redo','General',1
EXEC [dbo].pInsTranslation 'Bold',N'غامق','Bold','General',1
EXEC [dbo].pInsTranslation 'Italic',N'مائل','Italic','General',1
EXEC [dbo].pInsTranslation 'Underline',N'تحته خط','Underline','General',1
EXEC [dbo].pInsTranslation 'Strikethrough',N'يتوسطه خط','Strikethrough','General',1
EXEC [dbo].pInsTranslation 'Align_Left',N'محاذاة إلى اليسار','Align Left','General',1
EXEC [dbo].pInsTranslation 'Align_Centre',N'محاذاة إلى الوسط','Align Centre','General',1
EXEC [dbo].pInsTranslation 'Align_Right',N'محاذاة إلى اليمين','Align Right','General',1
EXEC [dbo].pInsTranslation 'Justify',N'ضبط','Justify','General',1
EXEC [dbo].pInsTranslation 'Indent',N'مسافة بادئة','Indent','General',1
EXEC [dbo].pInsTranslation 'Outdent',N'إلغاء المسافة البادئة','Outdent','General',1
EXEC [dbo].pInsTranslation 'Bullet_List',N'قائمة نقطية','Bullet List','General',1
EXEC [dbo].pInsTranslation 'Ordered_List',N'قائمة مرتبة','Ordered List','General',1
EXEC [dbo].pInsTranslation 'Text_Color',N'لون النص','Text Color','General',1
EXEC [dbo].pInsTranslation 'Background_Color',N'لون الخلفية','Background Color','General',1
EXEC [dbo].pInsTranslation 'Remove_Styling',N'إزالة التنسيق','Remove Styling','General',1
EXEC [dbo].pInsTranslation 'Subscript',N'منخفض','Subscript','General',1
EXEC [dbo].pInsTranslation 'Superscript',N'مرتفع','Superscript','General',1
EXEC [dbo].pInsTranslation 'Link',N'رابط','Link','General',1
EXEC [dbo].pInsTranslation 'Web_Address',N'عنوان الويب','Web Address','General',1
EXEC [dbo].pInsTranslation 'Text',N'نص','Text','General',1
EXEC [dbo].pInsTranslation 'Open_in_new_window',N'فتح في نافذة جديدة','Open in new window','General',1
EXEC [dbo].pInsTranslation 'Unlink',N'إزالة الرابط','Unlink','General',1
EXEC [dbo].pInsTranslation 'Font',N'خط','Font','General',1
EXEC [dbo].pInsTranslation 'Font_Size',N'حجم الخط','Font Size','General',1
EXEC [dbo].pInsTranslation 'Format_Block',N'كتلة التنسيق','Format Block','General',1 
 ------------24 sep-2024
 EXEC [dbo].pInsTranslation 'Plantiff_Name',N'إسم المدعي','Plaintiff Name','CMS',1
EXEC [dbo].pInsTranslation 'Is_FinalJudgment',N'تعيين كحكم نهائي؟','Set as Final Judgment?','CMS',1
EXEC [dbo].pInsTranslation 'Document_Preview',N'معاينة المستند','Document Preview','DMS',1
EXEC [dbo].pInsTranslation 'Borrow_Return_Books',N'إستعارة/إرجاع الكتب','Borrow/Return Books','Borrowed Screen',1
EXEC [dbo].pInsTranslation 'Select_Employee',N'إختر الموظف','Select Employee','Borrowed Screen',1
EXEC [dbo].pInsTranslation 'Borrow_Success',N'تم استعارة الكتاب بنجاح','Book has been borrowed successfully','Borrowed Screen',1
EXEC [dbo].pInsTranslation 'Search_Book',N'بحث عن كتاب','Search Book','Borrowed Screen',1
EXEC [dbo].pInsTranslation 'Add_Borrow_Literature',N'إضافة كتاب للاستعارة','Add Book to Borrow','Borrowed Screen',1
EXEC [dbo].pInsTranslation 'Barcode_Required',N'أدخل رقم الباركود','Enter Barcode Number','Borrowed Screen',1
EXEC [dbo].pInsTranslation 'Please_Select_CivilId_Or_UserId',N'يرجى إختيار اسم الموظف أو ادخل الرقم المدني','Please select employee name or enter the civil ID','Borrowed Screen',1
EXEC [dbo].pInsTranslation 'Book_not_Available',N'الكتاب غير متوفر','Book is not available','Borrowed Screen',1
EXEC [dbo].pInsTranslation 'Please_Select_One_CivilId_Or_Employee_Name',N'يرجى اختيار إسم موظف واحد أو أدخل الرقم المدني','Select one employee name or enter the civil ID','Borrowed Screen',1
EXEC [dbo].pInsTranslation 'User_Borrow_History',N'التغيرات على استعارة الكتب','Borrow Books History','Borrowed Return Screen',1
EXEC [dbo].pInsTranslation 'ISBN_Number',N'الرقم العالمي الموحد للكتاب','ISBN Number','Borrowed Return Screen',1
EXEC [dbo].pInsTranslation 'Sure_Submit_Mark_As_Important',N'هل ترغب في وضع علامة أهمية على هذا الملف؟','Do you wish to mark this case file as important?','General',1
EXEC [dbo].pInsTranslation 'Sure_Submit_Mark_As_Un_Important',N'هل تود تغيير ملف القضية الى غير مهمة؟','Do you wish to mark this case file as unimportant?','General',1
EXEC [dbo].pInsTranslation 'Confirm_Action',N'تأكيد العملية','Confirm Action','General',1

  EXEC [dbo].pInsTranslation 'Sure_Returned_The_Record',N'هل أنت متأكد أنك تريد ارجاع الكتاب؟','Are you sure you want to return the book?','Borrowed Return Screen',1

EXEC [dbo].pInsTranslation 'Return_Success',N'تم ارجاع الكتاب بنجاح','Book has been returned successfully','Borrowed Return Screen',1

EXEC [dbo].pInsTranslation 'Extend_Borrow_Rquest_Success',N'تم تمديد فترة استعارة الكتاب بنجاح','Book borrowing period has been extended successfully ','Borrowed Return Screen',1
EXEC [dbo].pInsTranslation 'Author_Name',N'اسم المؤلف','Author Name','Literature',1

EXEC [dbo].pInsTranslation 'Extension',N'الرقم الداخلي','Extension Number','Borrow Return Screen',1

/*<History Author='Ammaar Naveed' Date='02-10-2024'>Employee delegation translations</History>*/
EXEC [dbo].pInsTranslation 'Employee_Delegation',N'تفويض الموظفين','Employee Delegation','Employee Delegation',1
EXEC [dbo].pInsTranslation 'Employee_Leave_Delegation',N'تفويض عن الموظفين المجازين','Employee Leave Delegation','Employee Delegation',1
EXEC [dbo].pInsTranslation 'Employee_Tasks_Delegation',N'تفويض مهام الموظفين','Employee Tasks Delegation','Employee Delegation',1
EXEC [dbo].pInsTranslation 'Tasks_Assigned_Successfully',N'تم تحويل المهام بنجاح','Tasks Transferred Successfully','Employee Delegation',1
EXEC [dbo].pInsTranslation 'Delegated_Employee',N'الموظف المفوض','Delegated Employee','Employee Delegation',1
EXEC [dbo].pInsTranslation 'Users_List',N'قائمة المستخدمين','Users List','Group permission grid',1

/*<History Author='Ammaar Naveed' Date='02-10-2024'>Employee delegation translations</History>*/
EXEC [dbo].pInsTranslation 'Employee_Delegation_History',N'التغيرات على تفويض الموظفين','Employee Delegation History','Sector Users List',1
EXEC [dbo].pInsTranslation 'Delegated_By',N'تم التفويض من قبل','Delegated By','Employee Delegation',1

/*<History Author='Ammaar Naveed' Date='08-10-2024'>User Role Screen</History>*/
EXEC [dbo].pInsTranslation 'Please_Select_Role',N'يرجى اختيار الدور للموظفين المختارين','Please select a role for selected employees','User Role Screen',1
EXEC [dbo].pInsTranslation 'Employee_Password_History',N'التغيرات على كلمة المرور','Employee Password History','User Detail Screen',1
EXEC [dbo].pInsTranslation 'Please_Select_Users',N'يرجى اختيار موظفين اثنين على الأقل  لتعيين الدور','Please select at least 2 employees in order to assign role',1
EXEC [dbo].pInsTranslation 'Sector_Type',N'نوع القطاع','Sector type','User Detail Page',1
EXEC [dbo].pInsTranslation 'Employee_Status_Updated_Success',N'تم تعديل حالة الموظف بنجاح','Employee status has been updated successfully','Edit employee',1
EXEC [dbo].pInsTranslation 'Group_Update_Success_Message',N'تم تعديل المجموعة بنجاح','Group has been updated successfully','Edit Group',1

EXEC [dbo].pInsTranslation 'Username_Required',N'يجب إدخال كلمة المرور','Username must be entered','Login',1
EXEC [dbo].pInsTranslation 'Password_Required',N'يجب إدخال كلمة المرور','Password must be entered','Login',1
EXEC [dbo].pInsTranslation 'Please_Add_Filter',N'الرجاء إدخال قيمة لعامل تصفية واحد على الأقل','Please enter a value for at least one filter','General',1

/*<History Author='Ammaar Naveed' Date='10-10-2024'>Assign supervisor screen</History>*/
EXEC [dbo].pInsTranslation 'Select_Manager',N'اختر اسم المسؤول المباشر','Select Manager','Assign Supervisor & Manager Screen',1
EXEC [dbo].pInsTranslation 'Assign_Supervisor_And_Manager',N'تعيين المسؤول المباشر والمشرف الفني','Assign Supervisor & Manager','Assign Supervisor & Manager Screen',1

EXEC [dbo].pInsTranslation 'Employee_Permanently_Delegated',N'تم تفويض الموظف بشكل دائم','Employee Permanently Delegated','Sector User Screen',1


EXEC [dbo].pInsTranslation 'Decision_Is_Required',N'مطلوب القرار!','Decision is required.','General',1
EXEC [dbo].pInsTranslation 'Please_Enter_Civil_ID',N'يرجى ادخال الرقم المدني','Please enter civil id','MobileApplication',1
EXEC [dbo].pInsTranslation 'Final_Document_Detail',N'Final Document Detail','Final Document Detail','COMS',1



EXEC [dbo].pInsTranslation 'Start_Date',N'تاريخ البدء','Start Date','Create Mask',1
EXEC [dbo].pInsTranslation 'Upcoming_And_Current_Hearings',N'رول الجلسات تم تعيينها','Upcoming And Current Hearings','SideMenuBar',1
EXEC [dbo].pInsTranslation 'Required_Case_Number',N'يجب ادخال رقم القضية','Case Number is Required','CMS',1
EXEC [dbo].pInsTranslation 'Enter_Valid_CivilId',N'الرقم مدني غير صحيح','Invalid Civil ID!','MOJ Registration',1
EXEC [dbo].pInsTranslation 'Change_Language',N'تغيير اللغة','Change Language','MobileApp',1
EXEC [dbo].pInsTranslation 'Please_Select_Year',N'يرجى احتيار السنة','Please select year','MobileApp',1
EXEC [dbo].pInsTranslation 'Please_Enter_Name',N'يرجى ادخال الاسم','Please enter name','MobileApp',1
EXEC [dbo].pInsTranslation 'Logout_Confirmation',N'هل انت متأكد انو تود تسجيل الخروج؟','Are you sure you want to Logout?','MobileApp',1




EXEC [dbo].pInsTranslation 'App_Exit_Confirmation',N'يرجى الضغط مرة أخرى للخروج','Please click back again to exit','MobileApp',1
EXEC [dbo].pInsTranslation 'Select_Session_Date',N'اختر تاريخ الجلسة','Select Hearing Date','MobileApp',1
EXEC [dbo].pInsTranslation 'Must_Select_Chambers_Numbers',N'اختر أرقام الدوائر','Please select Chamber Number','MobileApp',1
EXEC [dbo].pInsTranslation 'Select_Roll',N'اختر الرول','Select Roll','MobileApp',1


---- 15-10-2024 ----

EXEC [dbo].pInsTranslation 'File_Maximum_Size',N'الحد الأقصى لحجم الملف:','Maximum File Size:','Edit System Setting page',1 
EXEC [dbo].pInsTranslation 'Mega_Byte',N'ميجا بايت','Mega Byte','Edit System Setting page',1 

EXEC [dbo].pInsTranslation 'Sure_Change_File',N'هل تريد تغيير خطاب التفويض الخاص بك؟','Do you want to change your authorization letter?','Edit System Setting page',1 
EXEC [dbo].pInsTranslation 'Change_File',N'تغيير الملف','Change File','Edit System Setting page',1 
--- end ---
EXEC [dbo].pInsTranslation 'Logout_Confirmation',N'هل انت متأكد انو تود تسجيل الخروج؟','Are you sure you want to Logout?','MobileApp',1

EXEC [dbo].pInsTranslation 'Select_Sector_And_Employee_Type',N'اختر القطاع ونوع الموظف أولاً','Select Sector & Employee Type First','Admin Groups',1
EXEC [dbo].pInsTranslation 'Select_User_For_Search',N'اختر المستخدم لتفعيل البحثاً','Select User To Activate Search','Admin Groups',1
EXEC [dbo].pInsTranslation 'Group_Users',N'مستخدمو المجموعة','Group Users','Admin Groups',1

/*<History Author='Ammaar Naveed' Date='22-10-2024'>Correspondence Detail View</History>*/
EXEC [dbo].pInsTranslation 'Consultation_File_Date',N'تاريخ ملف الاستشاري','Consultation File Date','Correspondence Detail View',1

/*<History Author='Ammaar Naveed' Date='23-10-2024'>Correspondence Detail View</History>*/
EXEC [dbo].pInsTranslation 'Consultation_Request_Date',N'تاريخ طلب الاستشاري','Consultation Request Date','Correspondence Detail View',1
EXEC [dbo].pInsTranslation 'Only_Arabic_Characters_Are_Allowed',N'يسمح فقط بالأحرف العربية','Only Arabic characters are allowed in Arabic fields','Add/Edit Group',1
EXEC [dbo].pInsTranslation 'Only_English_Characters_Are_Allowed',N'يسمح فقط بالأحرف الإنجليزية','Only English characters are allowed in English fields','Add/Edit Group',1
EXEC [dbo].pInsTranslation 'Please_Select_Group_Type',N'يرجى اختيار نوع المجموعة','Please select group type','Add/Edit Group',1
EXEC [dbo].pInsTranslation 'Please_Assign_Permissions',N'يرجى تعيين صلاحية للمجموعة','Please assign permissions to group','Add/Edit Group',1









EXEC [dbo].pInsTranslation 'Required_Document',N'الرجاء تحميل المستند المطلوب','Please upload required document.','General',1
EXEC [dbo].pInsTranslation 'Required_Field',N'هذه الخانة مطلوبه.','Required field.','General',1


EXEC [dbo].pInsTranslation 'CAN_Max_Limit_9',N'يجب أن يكون الرقم الألي للقضية 9 أرقام	','CAN number must be 9 digits.','General',1
EXEC [dbo].pInsTranslation 'Only_digits_and_slashes_are_allowed',N'يُسمح فقط بالأرقام والخطوط المائلة','Only digits and slashes are allowed.','General',1
EXEC [dbo].pInsTranslation 'Only_Specified_Characters_allowed',N'يسمح فقط  أحرف خاصة','Only special characters allowed.','General',1
EXEC [dbo].pInsTranslation 'Only_English_Character_allowed',N'يجب ادخال أحرف إنجليزية فقط','Only english characters allowed.','General',1
EXEC [dbo].pInsTranslation 'Arabic_Name_Is_Allowed',N'يجب ادخال أحرف عربية فقط','Only arabic characters allowed','General',1
EXEC [dbo].pInsTranslation 'Invalid_Title',N'يجب ادخال الاسم بالانجليزي','Name must be entered in english.','General',1
EXEC [dbo].pInsTranslation 'Required_Valid_Email',N'مطلوب بريد إلكتروني صالح','Required valid email','General',1
EXEC [dbo].pInsTranslation 'RequiredDocument',N'الرجاء تحميل المستند المطلوب','Please upload required document.','General',1
EXEC [dbo].pInsTranslation 'Required_Field',N'هذه الخانة مطلوبه.','Required field.','General',1
EXEC [dbo].pInsTranslation 'Select_Roll',N'اختر الرول','Select Roll','MobileApp',1

EXEC [dbo].pInsTranslation 'Add_Defendant',N'إضافة الطرف','Add Defendant','G2G',1
EXEC [dbo].pInsTranslation 'List_Case_Parties',N'قائمة الأطراف القضية','List Of Case Parties','G2G Case Management',1

EXEC [dbo].pInsTranslation 'Required_Field',N'هذه الخانة مطلوبه.','Required field.','General',1
EXEC [dbo].pInsTranslation 'Logout_Confirmation',N'هل انت متأكد انو تود تسجيل الخروج؟','Are you sure you want to Logout?','MobileApp',1
EXEC [dbo].pInsTranslation 'Select_Request_Type',N'اختر نوع الطلب','Select Request Type','Case Request',1
EXEC [dbo].pInsTranslation 'Date_Submission_CSC',N'تاريخ الارسال للخدمة المدنية','Date Submission to CSC','General',1
EXEC [dbo].pInsTranslation 'Consultation_Pledge_Statement',N'أوافق بموجب هذا على شروط وأحكام الفتوى والتشريع ','I hereby agree to the terms and conditions of FATWA','consultation View Page',1
EXEC [dbo].pInsTranslation 'Template_Preview',N'معاينة القالب','Template Preview','Add Consultation Request Form Page',1
EXEC [dbo].pInsTranslation 'Template',N'القالب','Template','consultationrequest add',1
EXEC [dbo].pInsTranslation 'CivilId',N'رقم البطاقة المدنية','Civil Id','Add Case Party',1
EXEC [dbo].pInsTranslation 'Legislation_File_Type',N'نوع التشريع','Legislation File Type','Add Consultation Request Form Page',1
EXEC [dbo].pInsTranslation 'CheckBox_Required',N'يجب تحديد خانة الاختيار هذه.','This checkbox must be selected.','General',1

------------------- Return Inventory - Date : 29-Oct-024
/*<History Author='Ammaar Naveed' Date='29-10-2024'>Create Case File Page translations</History>*/
EXEC [dbo].pInsTranslation 'File',N'ملف','File','Create Case File Page',1

EXEC [dbo].pInsTranslation 'Return_Inventory_Request',N'طلب إرجاع المخزون','Return Inventory Request','Return Inventory',1
  EXEC [dbo].pInsTranslation 'Mega_Byte',N'ميجا بايت','MB','General',1

/*<History Author='Ammaar Naveed' Date='30-10-2024'>Add/Edit group page</History>*/
EXEC [dbo].pInsTranslation 'English',N'انجليزي','English','Add/Edit Group',1
EXEC [dbo].pInsTranslation 'Arabic',N'عربي','Arabic','Add/Edit Group',1
EXEC [dbo].pInsTranslation 'Case_Request_Pledge_Statement',N'أوافق بموجب هذا على شروط وأحكام الفتوى والتشريع ','I hereby agree to the terms and conditions of FATWA','CMS',1
 
/*<History Author='Arshad khan' Date='26-08-2024'>Meeting MoM</History>*/

 EXEC [dbo].pInsTranslation 'Attendee_Decision',N'MOM Review Decision','MOM Review Decision','Meeting MoM',1

/*<History Author='Ammaar Naveed' Date='14-11-2024'>Missing translation for add bulk role</History>*/
 EXEC [dbo].pInsTranslation 'Role_For_Selected_Employees_Updated_Successfully',N'تم تحديث الدور للموظفين المحددين بنجاح','Role for selected employees has been updated successfully','Assign employee role',1
 EXEC [dbo].pInsTranslation 'Add_Consultation_File',N'Add Consultation File','Add Consultation File','COMS',1
 EXEC [dbo].pInsTranslation 'Edit_Consultation_File',N'Edit Consultation File','Edit Consultation File','COMS',1
 EXEC [dbo].pInsTranslation 'View_Employee',N'View Employee','View Employee','EP',1
 EXEC [dbo].pInsTranslation 'CivilId',N'رقم البطاقة المدنية','Civil ID','EP',1
 EXEC [dbo].pInsTranslation 'Moj_Statistics',N'إحصائيات وزارة العدل','MOJ Statistics','ODRP',1
 EXEC [dbo].pInsTranslation 'Consultation_File_Rejected_By_Lawyer',N'Consultation File has been rejected by lawyer.','Consultation File has been rejected by lawyer.','COMS',1
EXEC [dbo].pInsTranslation 'Pre_Court_Type',N'نوع الدائرة','Chamber Type','Create Request Form',1

EXEC [dbo].pInsTranslation 'Transfer_Request_Initiated',N'تم إحالة الطلب بنجاح','Transfer request has been initiated','Common',1
EXEC [dbo].pInsTranslation 'Workflow_Added_Successfully',N'تم تقديم آلية العمل بنجاح','Workflow has been submitted successfully','Workflow',1
EXEC [dbo].pInsTranslation 'Sure_Borrow_Book',N'هل أنت متأكد أنك تريد استعارة هذا الكتاب؟','Are you sure you want to borrow this book?','LLS',1
EXEC [dbo].pInsTranslation 'Confirm_Selection',N'تأكيد العملية','Confirm S	election','LLS',1


------- 16-12-2024
EXEC [dbo].pInsTranslation 'Inactive',N'غير نشط','Inactive','Process List',1

EXEC [dbo].pInsTranslation 'Launched_Date',N'تاريخ الانطلاق','Launched Date','Process List',1

EXEC [dbo].pInsTranslation 'Process_Code',N'رقم العملية','Process Code','Process List',1

EXEC [dbo].pInsTranslation 'Resources_List',N'قائمة الموارد','Resources List','Process List',1

EXEC [dbo].pInsTranslation 'Total_Queue_items',N'إجمالي قائمة الانتظار','Total Queue Items','Automation Monitoring Screen',1

EXEC [dbo].pInsTranslation 'Current_Session_Date',N'تاريخ الدورة الأخيرة','Recent Session Date','Automation Monitoring Screen',1

EXEC [dbo].pInsTranslation 'Service_Request_Flow_Setup',N'إعداد تدفق طلب الخدمة','Service Request Flow Setup','Automation Monitoring Screen',1


EXEC [dbo].pInsTranslation 'Service_Request_Approval_Flow_List',N'قائمة تدفق الموافقة على طلب الخدمة','Service Request Approval Flow List','Automation Monitoring Screen',1


------ 16-12-2024
EXEC [dbo].pInsTranslation 'Confirm_Selection',N'تأكيد العملية','Confirm S	election','LLS',1
EXEC [dbo].pInsTranslation 'Pre_Court_Type',N'نوع الدائرة','Chamber Type','Create Request Form',1

/*<History Author='Ali Hassan' Date='17-12-2024'>Missing translation for Archived Cases List</History>*/
EXEC [dbo].pInsTranslation 'Archived_Cases',N'الحالات المؤرشفة','Archived Cases','archivedcases-list',1
EXEC [dbo].pInsTranslation 'Archived_Cases_List',N'لائحة الحالات المؤرشفة','Archived Cases List','archivedcases-list',1

-------------------------------- phase 1 issues UAT
EXEC [dbo].pInsTranslation 'CivilId/CRN',N'الرقم المدني/السجل التجاري','Civil ID/CRN','Civil ID/CRN',1
EXEC [dbo].pInsTranslation 'Amount_Should_Be_Between',N'يجب أن يكون المبلغ بين','Amount should be between','CMS',1
EXEC [dbo].pInsTranslation 'Add_Case_Party',N'يجب أن يكون المبلغ بين','Add Case Party','CMS',1

------------------- 11-6-2024 --------------------------


----------------------------
EXEC [dbo].pInsTranslation 'Bug_Ticket_Details',N'عرض تفاصيل التذكرة','View Ticket Details','Detail Ticket',1
EXEC [dbo].pInsTranslation 'Raise_A_Ticket',N'إنشاء تذكرة','Raise A Ticket','view bugticket',1
EXEC [dbo].pInsTranslation 'Resolved_By',N'تم الحل من قبل','Resolved By','view bugticket',1
EXEC [dbo].pInsTranslation 'Ticket_Resolution',N'القرار','Resolution','Detail Ticket',1
EXEC [dbo].pInsTranslation 'Rating',N'تصنيف','Rating','Detail Ticket',1
EXEC [dbo].pInsTranslation 'Ticket_Id',N'رقم الطلب','Ticket Id','List Ticket',1
EXEC [dbo].pInsTranslation 'Assigned_Group',N'المجموعة التي تم تعيينها','Assigned Group','Detail Ticket',1
EXEC [dbo].pInsTranslation 'Portal',N'البوابة','Portal','Detail Ticket',1
EXEC [dbo].pInsTranslation 'Ticket_Related_To_Bug',N'إذا كانت هذه التذكرة مرتبطة بأي خطأ، يرجى تحديد الخطأ','If this ticket is related to any bug,please select the bug.','Ticket Assignment',1
EXEC [dbo].pInsTranslation 'Related_Bugs',N'الأخطاء ذات الصلة','Related Bugs','Ticket Assignment',1
EXEC [dbo].pInsTranslation 'Properties_Of_Ticket',N'خصائص التذكرة','Properties Of Ticket','Ticket Assignment',1
EXEC [dbo].pInsTranslation 'Assign_To_Support_Team',N'تعيين الى فريق الدعم','Assign To Support Team','Ticket Assignment',1
EXEC [dbo].pInsTranslation 'Ticket_Tag_Assigned_Successfully',N'تم وضع علامة على التذكرة التي تحتوي على خطأ وتم تعيينها بنجاح','Ticket tagged with bug and assigned successfully','Ticket Assignment',1
EXEC [dbo].pInsTranslation 'FeedBack',N'ملاحظة ','FeedBack','view bugticket',1
EXEC [dbo].pInsTranslation 'Report_A_Bug',N'الإبلاغ عن مشكلة في النظام','Report a Bug','Add Reported Bug',1
EXEC [dbo].pInsTranslation 'Issue_Type_Configuration',N'اعدادات نوع المشكلة','Issue Type Configuration','Issue Type List',1
EXEC [dbo].pInsTranslation 'Add_Issue_Type',N'إضافة نوع المشكلة','Add New Issue Type','Issue Type List',1
EXEC [dbo].pInsTranslation 'User_Configuration_Type',N'إعدادات المستخدم لنوع المشكلة','User Configuration For Issue Type','Issue Type List',1
EXEC [dbo].pInsTranslation 'Properties_Configuration_Type',N'إعدادات الخصائص لنوع المشكلة','Properties Configuration For Issue Type','Issue Type List',1
EXEC [dbo].pInsTranslation 'Assigned_By',N'مكلف من قبل','Assigned By','Issue Type List',1
EXEC [dbo].pInsTranslation 'Archiving_Reports',N'Archiving Reports','Archiving Reports','CMS',1
EXEC [dbo].pInsTranslation 'CANs_And_Cases.xlsx',N'إجمالي الملفات الممسوحة والمؤرشفة  - (مرفق 1)','CANs and Cases.xlsx','CMS',1
EXEC [dbo].pInsTranslation 'Business_Exceptions.xlsx',N'إجمالي الملفات ذات الاستثناءات - (مرفق 2)','Business Exceptions.xlsx','CMS',1
EXEC [dbo].pInsTranslation 'Overall_CANs_Pushed_to_FATWAUAT.xlsx',N'الملفات غير القابلة للمعالجة (بدون  أرقام آلية، ملفات تالفة، إلخ) - (مرفق 3)','Overall CANs Pushed to FATWAUAT.xlsx','CMS',1

-----------------------------31-Dec-2024-----Muhammad Ali-----------Validation limits translation-------
EXEC [dbo].pInsTranslation 'Max_Ninety_Characters',N'النص يجب ألا يتجاوز 90 حرفًا','The text should not exceed 90 characters','Validation and Limits',1
EXEC [dbo].pInsTranslation 'Max_One_Fifty_Characters',N'النص يجب ألا يتجاوز 150 حرفًا','The text should not exceed 150 characters','Validation and Limits',1
EXEC [dbo].pInsTranslation 'Max_Hundred_Characters',N'النص يجب ألا يتجاوز 100 حرف','The text should not exceed 100 characters','Validation and Limits',1
EXEC [dbo].pInsTranslation 'Max_Thousand_Characters',N'النص يجب ألا يتجاوز 1000 حرف','The text should not exceed 1000 characters','Validation and Limits',1
EXEC [dbo].pInsTranslation 'Max_Thirty_Characters',N'النص يجب ألا يتجاوز 30 حرفًا','The text should not exceed 30 characters','Validation and Limits',1
EXEC [dbo].pInsTranslation 'Max_Three_Hundred_Characters',N'النص يجب ألا يتجاوز 300 حرف','The text should not exceed 300 characters','Validation and Limits',1
EXEC [dbo].pInsTranslation 'Max_Nine_Characters',N'النص يجب ألا يتجاوز 9 أحرف','The text should not exceed 9 characters','Validation and Limits',1
EXEC [dbo].pInsTranslation 'Max_Six_Characters',N'النص يجب ألا يتجاوز 6 أحرف','The text should not exceed 6 characters','Validation and Limits',1
EXEC [dbo].pInsTranslation 'Max_Thirty_Four_Characters',N'النص يجب ألا يتجاوز 34 حرفًا','The text should not exceed 34 characters','Validation and Limits',1
EXEC [dbo].pInsTranslation 'Max_Five_Hundred_Characters',N'النص يجب ألا يتجاوز 500 حرف','The text should not exceed 500 characters','Validation and Limits',1

----------------------Arshad-----------------
EXEC [dbo].pInsTranslation 'DS_STATUS',N'حالة التوقيع الإلكتروني','Digital Signature Status','DMS',1
EXEC [dbo].pInsTranslation 'Ticket_Assignment',N'تعيين التذكرة','Ticket Assignment','Detail Ticket',1
EXEC [dbo].pInsTranslation 'Assign_To_User',N'تعيين للمستخدم','Assign To User','Ticket Assignment',1
EXEC [dbo].pInsTranslation 'Assign_To_Group',N'تعيين للمجموعة','Assign To Group','Ticket Assignment',1
EXEC [dbo].pInsTranslation 'Add_Resolution',N'إضافة القرار','Add Resolution','Common',1
EXEC [dbo].pInsTranslation 'Ticket_Resolved_Successfully',N'تم حل التذكرة بنجاح','Ticket resolved successfully','Decision Status',1
EXEC [dbo].pInsTranslation 'Reopen_Close_Ticket',N'إعادة فتح/إغلاق التذكرة','Reopen/Close Ticket','Reopen/Close Ticket',1
EXEC [dbo].pInsTranslation 'Ticket_Reopened_Successfully',N'تم إعادة فتح التذكرة بنجاح','Ticket reopened successfully','Decision Status',1
EXEC [dbo].pInsTranslation 'Ticket_Rejected_Successfully',N'تم رفض التذكرة بنجاح','Ticket rejected successfully','Decision Status',1
EXEC [dbo].pInsTranslation 'Ticket_Close',N'إغلاق','Close','Detail Ticket',1
EXEC [dbo].pInsTranslation 'Ticket_Reopen',N'إعادة فتح','Reopen','Detail Ticket',1
EXEC [dbo].pInsTranslation 'Reject_Ticket',N'رفض التذكرة','Ticket Rejection','Decision Status',1
EXEC [dbo].pInsTranslation 'Bug_Id',N'رقم الخطأ','Bug Id','Common',1
EXEC [dbo].pInsTranslation 'View_Bug_Details',N'عرض تفاصيل الخطأ','View Bug Details','Bug Detail',1
-------------------31/12/24
EXEC [dbo].pInsTranslation 'Please_Add_Comment',N'يرجى إضافة تعليق','Please add comment','Detail',1
EXEC [dbo].pInsTranslation 'Comment_Updated_Successfully',N'تم تحديث التعليق بنجاح ','Comment updated successfully','view bugticket',1
EXEC [dbo].pInsTranslation 'Ticket_Accepted_Successfully',N'تم استلام التذكرة بنجاح','Ticket accepted successfully','Detail Ticket',1
EXEC [dbo].pInsTranslation 'Please_Add_Comment',N'يرجى إضافة تعليق','Please add comment','Common',1
EXEC [dbo].pInsTranslation 'Resolve',N'تم الحل','Resolved','Detail Ticket',1


-----------------------
EXEC [dbo].pInsTranslation 'No_Of_Approval',N'عدد الموافقات','No. of Approval','Case Request Detail',1
EXEC [dbo].pInsTranslation 'Sure_Submit',N'هل أنت متأكد أنك تريد الإرسال؟','Are you sure you want to submit?','Add Document page',1
EXEC [dbo].pInsTranslation 'Tagged_Bug',N'خطأ موسوم','Tagged Bug','Ticket Detail',1
EXEC [dbo].pInsTranslation 'No_record_found',N'لا يوجد سجلات','No record found','General',1
EXEC [dbo].pInsTranslation 'Tagged_Tickets',N'التذاكر الموسومة','Tagged Tickets','Bug Detail',1
EXEC [dbo].pInsTranslation 'Assign_TypeModule_Added_Successfully',N'تم تكوين الخصائص بنجاح','Properties has been configured successfully','Assign Type Module',1
EXEC [dbo].pInsTranslation 'Bug_Type_Assigned_User_Successfully',N'تم تكوين بيانات المستخدم بنجاح','The user has been configured successfully','UnAssign Bug Type To User',1


EXEC [dbo].pInsTranslation 'Confirm',N'تأكيد؟','Confirm?','General',1


EXEC [dbo].pInsTranslation 'Ticket_Closed_Successfully',N'تم إغلاق الطلب بنجاح','Ticket has been Closed Successfully','Add Bug Type',1

EXEC [dbo].pInsTranslation 'System_Setting',N'إعداد النظام','System Setting','Edit System Setting page',1
EXEC [dbo].pInsTranslation 'System_Setting_Title',N'تحرير إعداد النظام','Edit System Setting','Edit System Setting page',1
EXEC [dbo].pInsTranslation 'Grid_Pagination',N'ترقيم الشبكة','Grid Pagination','Edit System Setting page',1
EXEC [dbo].pInsTranslation 'Number_Enter_placeholder',N'الرجاء إدخال الرقم','Please Entere Number','Edit System Setting page',1
EXEC [dbo].pInsTranslation 'Number_is_required',N'الرقم مطلوب','Number is required','Edit System Setting page',1
EXEC [dbo].pInsTranslation 'Book_CopyCount',N'عدد نسخ الكتاب','Book Copy Count','Edit System Setting page',1
EXEC [dbo].pInsTranslation 'Eligible_Count',N'العدد المؤهل','Eligible Count','Edit System Setting page',1
EXEC [dbo].pInsTranslation 'Borrow_Period',N'فترات الاستعارة','Borrow Periods','Edit System Setting page',1
EXEC [dbo].pInsTranslation 'Extension_Period',N'فترات التمديد','Extension Periods','Edit System Setting page',1
EXEC [dbo].pInsTranslation 'File_Minimum_Size',N'أقل حجم للملف','File Minimum Size','Edit System Setting page',1
EXEC [dbo].pInsTranslation 'File_Maximum_Size',N'أكبر حجم للملف','File Maximum Size','Edit System Setting page',1
EXEC [dbo].pInsTranslation 'In_Kilobytes',N'بالكيلوبايت','In KiloByte','Edit System Setting page',1
EXEC [dbo].pInsTranslation 'Setting_Confirmation_Message',N'هل أنت متأكد من تحديث هذا الإعداد؟','Are you sure to update this setting?','Edit System Setting page',1
EXEC [dbo].pInsTranslation 'Setting_Update_Success_Messsage',N'تم تحديث إعداد النظام بنجاح','System setting updated successfully','System Configuration edit page',1
EXEC [dbo].pInsTranslation 'Reference_Screen',N'الشاشة المرجعية','Reference Screen','General',1


EXEC [dbo].pInsTranslation 'Save_failed_duplicate_data_detected',N'فشل الحفظ: تم اكتشاف بيانات مكررة','Save failed: duplicate data detected','general',1

EXEC [dbo].pInsTranslation 'Are_you_Sure_You_Want_to_Delete_Government_Entity',N'هل انت متأكد انك تريد حذف الجهة الحكومية؟','Are you Sure You Want to Delete Government Entity','CMS lookup',1


EXEC [dbo].pInsTranslation 'Full_Name',N'الاسم الكامل','Full Name','Assign Lawyer To Court',1 
EXEC [dbo].pInsTranslation 'Sector_Name',N'اسم القطاع','Sector Name','Add Group',1 
EXEC [dbo].pInsTranslation 'Create_Group_Type',N'إنشاء نوع المجموعة','Create Group Type','GroupType list',1 
EXEC [dbo].pInsTranslation 'Assign_Sector',N'تعيين القطاع','Assign Sector','Lookups',1 
EXEC [dbo].pInsTranslation 'Sure_Submit_Close_Ticket',N'هل أنت متأكد أنك تريد إغلاق هذا التذكرة؟','Are you sure you want to close this ticket?','Add Ticket',1 

EXEC [dbo].pInsTranslation 'IT_Support_Team',N'فريق دعم تكنولوجيا المعلومات','IT Support Team','Detail Bug Ticket',1

EXEC [dbo].pInsTranslation 'Must_Eight_Digits',N'لا يمكن لرقم القضية أن يكون أقل من 8 أرقام','Case No. must not be less than 8 digits','CMS',1
EXEC [dbo].pInsTranslation 'Login_Error_Title',N'بيانات المستخدم غير صحيحة','User data is incorrect','UMS',1
EXEC [dbo].pInsTranslation 'Go_Back',N'عودة','Back','CMS',1

EXEC [dbo].pInsTranslation 'Digital_Signature_Permissions',N'أذونات التوقيع الرقمي','Digital Signature Permissions','User Group Page',1
EXEC [dbo].pInsTranslation 'Vendor_Contract',N'عقد المورد','Vendor Contract','User Group Page',1
EXEC [dbo].pInsTranslation 'UMS_Configuration',N'إعدادات نظام إدارة المستخدمين','UMS Configuration','User Group Page',1
EXEC [dbo].pInsTranslation 'WebSystems_Save_Successfully',N'تم حفظ أنظمة الويب بنجاح','Web systems saved successfully','User Group Page',1
EXEC [dbo].pInsTranslation 'Side_Menu',N'القائمة الجانبية','Side Menu','MainLayout Page',1
EXEC [dbo].pInsTranslation 'FATWA_Admin_Portal',N'بوابة إدارة الفتوى','Fatwa Admin Portal','Save Group Page',1
EXEC [dbo].pInsTranslation 'Service_Request',N'طلب الخدمة','Service Request','Save Group Page',1
EXEC [dbo].pInsTranslation 'Claim_Updated',N'تم تحديث المطالبة بنجاح','Claim updated successfully','Save Claim Page',1
EXEC [dbo].pInsTranslation 'Update_Claims',N'قم بتحديث المطالبات','Update Claims','Save Claim Page',1
EXEC [dbo].pInsTranslation 'GroupAccessType_Updated_Successfully',N'تم تحديث نوع الوصول للمجموعة بنجاح.','The group access type was updated successfully.','Save Group Page',1
EXEC [dbo].pInsTranslation 'Sure_Save_Group_Type',N'هل أنت متأكد أنك تريد حفظ نوع المجموعة؟','Are you sure you want to save group type?','Save Group Type Page',1
EXEC [dbo].pInsTranslation 'Title_En',N'العنوان (الإنجليزية)','Title (English)','Update Claims Page',1
EXEC [dbo].pInsTranslation 'Title_Ar',N'العنوان (عربي)','Title (Arabic)','Update Claims Page',1
EXEC [dbo].pInsTranslation 'Add_Service_Request_Approval_WorkFlow',N'إضافة سير عمل الموافقة على طلب الخدمة','Add Service Request Approval WorkFlow','Update Claims Page',1
EXEC [dbo].pInsTranslation 'Level_Of_Approval',N'مستوى الموافقة','Level of Approval','Update Claims Page',1
EXEC [dbo].pInsTranslation 'Successfully_Added_Holidays',N'تمت إضافة العطلات الرسمية بنجاح.','Public holidays have been added successfully.','Update Claims Page',1
EXEC [dbo].pInsTranslation 'Worker_Service_Execution_Detail',N'تفاصيل تنفيذ خدمة العامل','Worker Service Execution Detail','Worker Service Page',1
EXEC [dbo].pInsTranslation 'Bank_Details',N'تفاصيل البنك','Bank Details','Update Claims Page',1
EXEC [dbo].pInsTranslation 'Representative_Code',N'الرمز التمثيلي','Representative Code','Common Lookups',1
EXEC [dbo].pInsTranslation 'Government_Entity_Department_History',N'تاريخ إدارة الكيان الحكومي','Government Entity Department History','Common Lookups',1
EXEC [dbo].pInsTranslation 'Edit_Task_Type',N'تعديل نوع المهمة','Edit Task Type','Enums Lookups',1
EXEC [dbo].pInsTranslation 'Enums_Value',N'قيمة التعدادات','Enums Value','Enums Lookups',1
EXEC [dbo].pInsTranslation 'Edit_Sub_type_Detail',N'تعديل تفاصيل النوع الفرعي','Edit Sub Type Detail','Enums Lookups',1
EXEC [dbo].pInsTranslation 'Add_Sub_Types',N'إضافة أنواع فرعية','Add SubTypes','Enums Lookups',1
EXEC [dbo].pInsTranslation 'Edit_Grade_Type',N'تعديل نوع الدرجة','Edit Grade Type','Enums Lookups',1
EXEC [dbo].pInsTranslation 'Add_Grade_Type',N'إضافة نوع الدرجة','Add Grade Type','Enums Lookups',1
EXEC [dbo].pInsTranslation 'Edit_Grade',N'تعديل الدرجة','Edit Grade','Enums Lookups',1
EXEC [dbo].pInsTranslation 'Edit_Nationality',N'تعديل الجنسية','Edit Nationality','Enums Lookups',1
EXEC [dbo].pInsTranslation 'Add_Nationality',N'إضافة الجنسية','Add Nationality','Enums Lookups',1
EXEC [dbo].pInsTranslation 'Subject_En',N'الموضوع (الإنجليزية)','Subject (English)','Enums Lookups',1
EXEC [dbo].pInsTranslation 'Subject_Ar',N'الموضوع (العربية)','Subject (Arabic)','Enums Lookups',1
EXEC [dbo].pInsTranslation 'Sure_Save_Event',N'هل أنت متأكد أنك تريد حفظ هذا الحدث؟','Are you sure you want to save this event?','Enums Lookups',1
EXEC [dbo].pInsTranslation 'Case_Data_Extraction',N'استخراج بيانات الحالة','Case Data Extraction','Case Data Extraction',1

EXEC [dbo].pInsTranslation 'GroupAccessType_Added_Successfully',N'تمت إضافة نوع الوصول للمجموعة بنجاح.','The group access type was added successfully.','Save Group Page',1
EXEC [dbo].pInsTranslation 'Add_Public_Holiday',N'إضافة عطلة رسمية','Add Public Holiday','Worker Service Module',1
EXEC [dbo].pInsTranslation 'Edit_Time_Interval_(SLA)',N'تحرير الفاصل الزمني (SLA)','Edit Time Interval (SLA)','Worker Service Module',1
EXEC [dbo].pInsTranslation 'Reminders_will_be_sent_X_days_before_the_total_SLA_deadline_where_X_is_the_number_of_days_you_set',N'سيتم إرسال التذكيرات قبل X أيام من الموعد النهائي الإجمالي لاتفاقية مستوى الخدمة حيث يمثل X عدد الأيام التي حددتها','Reminders will be sent X days before the total SLA deadline where X is the number of days you set','Time Interval Module',1
EXEC [dbo].pInsTranslation 'Disable',N'تعطيل','Disable','Time Interval Module',1
EXEC [dbo].pInsTranslation 'Click_To_Sync_GEs_And_Departments',N'انقر للمزامنة مرة أخرى','Click To Sync again','Common Lookups',1
EXEC [dbo].pInsTranslation 'GEs_And_Departments_Synced_Alert',N'تمت مزامنة الوحدات العامة والأقسام على:','GEs and Departments were synced on: ','Common Lookups',1
EXEC [dbo].pInsTranslation 'Are_you_Sure_You_Want_to_Delete_G2G_Correspondences_Receiver?',N'هل أنت متأكد أنك تريد حذف مستقبل المراسلات G2G؟','Are you Sure You Want to Delete G2G Correspondences Receiver?','G2G Correspondences Receiver Detail',1
EXEC [dbo].pInsTranslation 'GEDepartment',N'دائرة الكيان الحكومي','GE Department','G2G Correspondences Receiver Detail',1
EXEC [dbo].pInsTranslation 'G2G_Correspondences_Receiver',N'جهاز استقبال المراسلات G2G','G2G Correspondences Receiver','G2G Correspondences Receiver Detail',1
EXEC [dbo].pInsTranslation 'Add_G2G_Correspondences_Receiver_Detail',N'إضافة تفاصيل متلقي المراسلات G2G','Add G2G Correspondences Receiver Detail','G2G Correspondences Receiver Detail',1
EXEC [dbo].pInsTranslation 'Government_Entity_Department_History',N'تاريخ إدارة الكيان الحكومي','Government Entity Department History','Common Lookups',1
EXEC [dbo].pInsTranslation 'Government_Entity_Department_History',N'تاريخ إدارة الكيان الحكومي','Government Entity Department History','Common Lookups',1
EXEC [dbo].pInsTranslation 'Representative_Designation_EN',N'تعيين الممثل (الإنجليزية)','Representative Designation (English)','Number Pattern',1
EXEC [dbo].pInsTranslation 'Representative_Designation_AR',N'تسمية الممثل (العربية)','Representative Designation (Arabic)','Number Pattern',1
EXEC [dbo].pInsTranslation 'Representative_Code',N'الرمز التمثيلي','Representative Code','Number Pattern',1
EXEC [dbo].pInsTranslation 'Government_Entity_Department_History',N'تاريخ إدارة الكيان الحكومي','Government Entity Department History','Common Lookups',1
EXEC [dbo].pInsTranslation 'Representative_Designation_EN',N'تعيين الممثل (الإنجليزية)','Representative Designation (English)','Number Pattern',1
EXEC [dbo].pInsTranslation 'Representative_Designation_AR',N'تسمية الممثل (العربية)','Representative Designation (Arabic)','Number Pattern',1
EXEC [dbo].pInsTranslation 'Representative_Code',N'الرمز التمثيلي','Representative Code','Number Pattern',1
EXEC [dbo].pInsTranslation 'GE_Code',N'رمز الجهة الحكومية','GE Code','Number Pattern',1
EXEC [dbo].pInsTranslation 'DMS_Enums',N'تعدادات DMS','DMS Enums','Number Pattern',1
EXEC [dbo].pInsTranslation 'Enums',N'تعدادات','Enums','Number Pattern',1
EXEC [dbo].pInsTranslation 'Add_Digital_Signature_Methods',N'إضافة طرق التوقيع الرقمي','Add Digital Signature Methods','Dms Lookups',1
EXEC [dbo].pInsTranslation 'Request_Updated_Successfully',N'تم تحديث الطلب بنجاح','Request Updated Successfully','Number Pattern',1
EXEC [dbo].pInsTranslation 'Case_Request_Number_Pattern',N'نمط رقم طلب القضية','Case Request Number Pattern','Number Pattern',1
EXEC [dbo].pInsTranslation 'SubType',N'النوع الفرعي','Sub Type','CMS Module',1
EXEC [dbo].pInsTranslation 'Case_Management_Lookups',N'عمليات البحث في إدارة الحالات','Case Management Lookups','Case Management Lookups',1
EXEC [dbo].pInsTranslation 'Courts_G2G_And_Fatwa',N'قائمة تاريخ عمليات البحث','Courts (G2G And Fatwa)','lookups History list',1
EXEC [dbo].pInsTranslation 'Court_Ar',N'نوع المحكمة','Court Type','Case Management Lookups',1
EXEC [dbo].pInsTranslation 'Chambers_Hearings_Days',N'أيام جلسات الاستماع في الغرف','Chambers Hearings Days','Chambers Hearings Days',1
EXEC [dbo].pInsTranslation 'Hearing_Days',N'أيام جلسات الاستماع','Hearings Days','Chambers Hearings Days',1
EXEC [dbo].pInsTranslation 'Chamber_Code',N'قانون الغرفة','Chamber Code','Case Management Lookups',1
EXEC [dbo].pInsTranslation 'Chamber_Code',N'قانون الغرفة','Chamber Code','Case Management Lookups',1
EXEC [dbo].pInsTranslation 'District',N'منطقة','District','Case Management Lookups',1
EXEC [dbo].pInsTranslation 'Legal_Principle_Type_History',N'تاريخ نوع المبدأ القانوني','Legal Principle Type History','lookups History list',1
EXEC [dbo].pInsTranslation 'Legal_Publication_Source_History',N'تاريخ مصدر النشر','Publication Source History','lookups History list',1
EXEC [dbo].pInsTranslation 'Literature_Tags_History',N'علامات الأدب التاريخ','Literature Tags History','lookups History list',1
EXEC [dbo].pInsTranslation 'Literature_Dewey_Number_Pattern',N'الأدب رقم ديوي نمط','Literature Dewey Number Pattern','lookups History list',1
EXEC [dbo].pInsTranslation 'Building',N'مبنى','Building','Case Management Lookups',1
EXEC [dbo].pInsTranslation 'Vice_HOS_Responsible_Assigned',N'نائب رئيس قسم الصحة المسؤول عن المحامين المعينين','Vice HOS Responsible for Assigned Lawyers','SectorTypeAdd',1
EXEC [dbo].pInsTranslation 'Vice_HOS_Responsible',N'نائب رئيس قسم الصحة مسؤول عن جميع المحامين','Vice HOS Responsible for All Lawyers','Sector Type Add Page',1
EXEC [dbo].pInsTranslation 'Vice_HOS_Approval',N'موافقة نائب رئيس قسم الصحة','Vice HOS Approval','Case Management Lookups',1
EXEC [dbo].pInsTranslation 'Gender_Updated_Successfully',N'تم تحديث الجنس بنجاح','Gender Updated Successfully','Nationality',1
EXEC [dbo].pInsTranslation 'Edit_Gender_Detail',N'تعديل تفاصيل الجنس','Edit Gender Detail','Nationality',1
EXEC [dbo].pInsTranslation 'Male',N'ذكر','Male','Case Management Lookups',1
EXEC [dbo].pInsTranslation 'UMS_Fixed_Enums',N'تعدادات UMS الثابتة','UMS Fixed Enums','Nationality',1
EXEC [dbo].pInsTranslation 'Nationalities ',N'جنسيات','Nationalities','UMS Lookups',1
EXEC [dbo].pInsTranslation 'Grade_Types ',N'أنواع الدرجات','Grade Types','UMS Lookups',1
EXEC [dbo].pInsTranslation 'Borrow_Return_Day_Duration ',N'مدة يوم الاقتراض والإرجاع','Borrow Return Day Duration','Ep Department',1
EXEC [dbo].pInsTranslation 'Create_Notification_Event_Template ',N'إنشاء قالب الإشعارات','Create Notification Template','Create Template Page',1
EXEC [dbo].pInsTranslation 'Template_List',N'قائمة القوالب','Template List','Document Template List',1
EXEC [dbo].pInsTranslation 'Default_Correspondance_Receiver',N'متلقي المراسلات الافتراضي','Default Correspondence Receiver','UMS',1
EXEC [dbo].pInsTranslation 'Allow_DS',N'السماح بالتوقيع الرقمي','Allow Digital Signature','DS',1
EXEC [dbo].pInsTranslation 'Update_allow_digital_signature_message',N'هل أنت متأكد من أنك تريد تحديث تفويض التوقيع الرقمي لمستخدمين محددين؟','Sure you want Update digital signature authorization for select users','Admin',1
EXEC [dbo].pInsTranslation 'Role_Updated',N'تم تحديث الدور بنجاح.','Role has been updated successfully.','UMS Roles',1
EXEC [dbo].pInsTranslation 'WebSystems_Required',N'مطلوب نظام الويب','Web System Required','Case Management Lookups',1


EXEC [dbo].pInsTranslation 'Sure_Active_Workflow_Suspend_Current',N'لتفعيل سير العمل الجديد سيتم وقف سير العمل الحالي، هل أنت متأكد من تفعيل سير العمل الجديد؟','Making this workflow active will "Suspend" the current active workflow for this trigger. Are you sure you want to activate this workflow? ','Workflow',1
EXEC [dbo].pInsTranslation 'Sure_Active_Workflow',N'هل أنت متأكد من تفعيل سير العمل الجديد؟','Are you sure you want to activate this workflow? ','Workflow',1
EXEC [dbo].pInsTranslation 'Govt_Entity_Opinion_Placeholder',N'رأي الجهة الحكومية','Government entity opinion','Add Consultation Request Screen',1
EXEC [dbo].pInsTranslation 'Reject_and_send_to_Ge',N'رفض وإرسالها إلى الجهة الحكومية','Reject and Send to GE','Meeting',1
EXEC [dbo].pInsTranslation 'Accept_and_send_to_Ge',N'قبول وإرسال إلى الجهة الحكومية','Accept and Send to GE','Meeting',1


EXEC [dbo].pInsTranslation 'Enter_CAN_Number_and_CaseNumber',N'أدخل رقم العلبة ورقم الحالة','Enter CAN And Case Number','Automation Monitoring Screen',1
EXEC [dbo].pInsTranslation 'Last_Updated', N'آخر تحديث', 'Last Updated', 'Automation Monitoring Screen', 1
EXEC [dbo].pInsTranslation 'Requested_By', N'تم الطلب بواسطة', 'Requested By', 'Automation Monitoring Screen', 1
EXEC [dbo].pInsTranslation 'Communication_Tab', N'التواصل', 'Communication', 'Fatwa Admin Permission Grid', 1
EXEC [dbo].pInsTranslation 'Vendor_Contract_SideMenu_Heading', N'إدارة عقدالمورد', 'Vendor Contract Management', 'Vendor Contract', 1
EXEC [dbo].pInsTranslation 'Organizing_Committee_SideMenu_Heading', N'اللجنة المنظمة', 'Organizing Committee', 'Fatwa Admin Permission Grid', 1
EXEC [dbo].pInsTranslation 'Are_you_Sure_you_want_to_update_Status', N'هل أنت متأكد أنك تريد تحديث الحالة؟', 'Are you sure you want to update status ?', 'Government Entity', 1

EXEC [dbo].pInsTranslation 'Add_Digital_Signature_Methods', N'إضافة طرق التوقيع الرقمي', 'Add Digital Signature Methods', 'Dms Lookups', 1
EXEC [dbo].pInsTranslation 'Approval', N'موافقة', 'Approval', 'Service request flow list', 1
EXEC [dbo].pInsTranslation 'Selected_Document_Type', N'نوع المستند المحدد', 'Selected Document Type', 'DMS Enums List', 1
EXEC [dbo].pInsTranslation 'Sub_Type_Updated_Successfully', N'تم تحديث النوع الفرعي بنجاح', 'Subtype updated successfully', 'GE lookups List', 1
EXEC [dbo].pInsTranslation 'Sub_Type_Added_Successfully', N'تمت إضافة النوع الفرعي بنجاح', 'Subtype added successfully', 'GE lookups List', 1
EXEC [dbo].pInsTranslation 'Sectors', N'القطاعات', 'Sectors', 'GE lookups List', 1

EXEC [dbo].pInsTranslation 'WebSystems_Updated_Successfully', N'تم تحديث أنظمة الويب بنجاح', 'Web systems have been updated successfully', 'Web system List', 1
EXEC [dbo].pInsTranslation 'Sla_Should_Be_Greater_then_first_duration', N'ينبغي أن تكون SLA أكبر من المدة الأولى', 'Sla should be greater than first duration', 'Time interval add page', 1
EXEC [dbo].pInsTranslation 'selected_date_range_conflicts_with_existing_holidays', N'يتعارض نطاق التاريخ المحدد مع العطلات الحالية', 'Selected date range conflicts with existing holidays', 'Public Holiday Page ', 1
EXEC [dbo].pInsTranslation 'GE_Department', N'قسم GE', 'GE Department', 'G2G Correspondences Receiver Detail', 1
EXEC [dbo].pInsTranslation 'Is_Digitally_Sign', N'هل التوقيع رقميا؟', 'Is Digitally Sign', 'Dms Lookups', 1
EXEC [dbo].pInsTranslation 'Assign_Signing_Methods', N'تعيين طرق التوقيع', 'Assign Signing Methods', 'Service request flow list', 1
EXEC [dbo].pInsTranslation 'Document_Type_Signing_Method_Added_Successfully', N'تمت إضافة طريقة توقيع نوع المستند بنجاح', 'Document type signing method added successfully', 'DMS enum list', 1
EXEC [dbo].pInsTranslation 'English_Name_Already_Exist', N'الاسم الانجليزي موجود بالفعل.', 'The english name already exists.', 'COMS Lookups', 1
EXEC [dbo].pInsTranslation 'Day_Pattern', N'نمط اليوم', 'Day Pattern', 'Number Pattern', 1
EXEC [dbo].pInsTranslation 'Month_Pattern', N'نمط الشهر ', 'Month Pattern', 'Number Pattern', 1
EXEC [dbo].pInsTranslation 'Year_Pattern', N'نمط السنة', 'Year Pattern', 'Number Pattern', 1
EXEC [dbo].pInsTranslation 'Government_Entity_Already_Exist', N'الكيان الحكومي المحدد لديه نمط بالفعل.', 'The specified government entity already has a pattern.', 'Number Pattern', 1
EXEC [dbo].pInsTranslation 'Add_Chamber_Number_Hearing_Detail', N'إضافة تفاصيل جلسة الاستماع برقم الغرفة', 'Add Chamber Number Hearing Detail', 'Chambers Hearings Days', 1
EXEC [dbo].pInsTranslation 'Judgments_Roll_Days', N'أيام صدور الأحكام', 'Judgments Roll Days', 'Chambers Hearings Days', 1
EXEC [dbo].pInsTranslation 'Hearings_Roll_Days', N'أيام لفة جلسات الاستماع', 'Hearings Roll Days', 'Chambers Hearings Days', 1
EXEC [dbo].pInsTranslation 'Book_Author', N'مؤلف الكتاب', 'Book Author', 'LLC lookups', 1
EXEC [dbo].pInsTranslation 'Full_Name_En', N'الاسم الكامل (الإنجليزية)', 'Full Name (English)', 'LLC lookups', 1
EXEC [dbo].pInsTranslation 'Full_Name_Ar', N'الاسم الكامل (عربي)', 'Full Name (Arabic)', 'LLC lookups', 1
EXEC [dbo].pInsTranslation 'Third_Name_En', N'الاسم الثالث (الإنجليزية)', 'Third Name (English)', 'Lms Literature Index page ', 1
EXEC [dbo].pInsTranslation 'Third_Name_Ar', N'الاسم الثالث (عربي)', 'Third Name (Arabic)', 'Lms Literature Index page ', 1
EXEC [dbo].pInsTranslation 'Book_Author_Updated_Successfully', N'تم تحديث مؤلف الكتاب بنجاح.', 'The book author has been updated successfully.', 'Lms Literature Index page ', 1
EXEC [dbo].pInsTranslation 'Edit_Book_Author', N'تعديل مؤلف الكتاب', 'Edit Book Author', 'Lms Literature Index page ', 1
EXEC [dbo].pInsTranslation 'Add_Book_Author', N'إضافة مؤلف الكتاب', 'Add Book Author', 'Lms Literature Index page ', 1
EXEC [dbo].pInsTranslation 'LastName_En', N'الاسم الأخير (انجليزي)', 'Last Name (English)', 'Lms Literature Index page ', 1
EXEC [dbo].pInsTranslation 'LastName_Ar', N'الاسم الأخير (عربي)', 'Last Name (Arabic)', 'Lms Literature Index page ', 1
EXEC [dbo].pInsTranslation 'Third_Name_En', N'الاسم الثالث (الإنجليزية)', 'Third Name (English)', 'Lms Literature Index page ', 1
EXEC [dbo].pInsTranslation 'Third_Name_Ar', N'الاسم الثالث (عربي)', 'Third Name (Arabic)', 'Lms Literature Index page ', 1
EXEC [dbo].pInsTranslation 'Are_you_Sure_You_Want_to_Delete_book_Author?', N'هل أنت متأكد أنك تريد حذف مؤلف الكتاب؟', 'Are you sure you want to delete the book author?', 'Lms Literature Index page ', 1
EXEC [dbo].pInsTranslation 'Nationalities', N'الجنسيات', 'Nationalities', 'Lms Literature Index page ', 1
EXEC [dbo].pInsTranslation 'Nationality_Updated_Successfully', N'تم تحديث الجنسية بنجاح.', 'Nationality updated successfully.', 'Nationality', 1
EXEC [dbo].pInsTranslation 'Nationality_Added_Successfully', N'تمت إضافة الجنسية بنجاح.', 'Nationality added successfully.', 'Nationality', 1
EXEC [dbo].pInsTranslation 'Add_Grade', N'أضف الصف', 'Add Grade', 'UMS lookups', 1
EXEC [dbo].pInsTranslation 'Grade_Added_Successfully', N'تمت إضافة الدرجة بنجاح.', 'The grade has been added successfully.', 'UMS lookups', 1
EXEC [dbo].pInsTranslation 'Grade_Updated_Successfully', N'تم تحديث الدرجة بنجاح.', 'The grade has been updated successfully.', 'UMS lookups', 1


---------------------------------------

EXEC [dbo].pInsTranslation 'Description_En',N'المحتوى (اللغة الانجليزية)','Description (English)','Add Literature Tag',1
EXEC [dbo].pInsTranslation 'Description_Ar',N'المحتوى (اللغة العربية)','Description (Arabic)','Add Literature Tag',1

EXEC [dbo].pInsTranslation 'Are_you_Sure_You_Want_to_Delete_Ep_Grade', N'هل أنت متأكد أنك تريد حذف الدرجة؟', 'Are you sure you want to delete the grade?', 'UMS lookups', 1
EXEC [dbo].pInsTranslation 'Are_you_Sure_You_Want_to_Delete_Ep_Grade_Type', N'هل أنت متأكد أنك تريد حذف نوع الدرجة؟', 'Are you sure you want to delete the grade type?', 'UMS lookups', 1
EXEC [dbo].pInsTranslation 'Add_Designation', N'إضافة التعيين', 'Add Designation', 'UMS lookups', 1
EXEC [dbo].pInsTranslation 'Add_Ep_Contract_Type', N'إضافة نوع عقد EP', 'Add EP Contract Type', 'UMS lookups', 1
EXEC [dbo].pInsTranslation 'Are_you_Sure_You_Want_to_Delete_Ep_Contract_Type?', N'هل أنت متأكد أنك تريد حذف نوع عقد EP؟', 'Are you sure you want to delete EP contract type?', 'UMS lookups', 1
EXEC [dbo].pInsTranslation 'Contract_Type_Added_Successfully', N'تمت إضافة نوع العقد بنجاح.', 'Contract type added Successfully.', 'UMS lookups', 1
EXEC [dbo].pInsTranslation 'Contract_Type_could_not_be_updated', N'لم يتم تحديث نوع العقد.', 'The contract type has not been updated.', 'UMS lookups', 1
EXEC [dbo].pInsTranslation 'Contract_Type_Updated_Successfully', N'تم تحديث نوع العقد بنجاح.', 'The contract type has been updated successfully.', 'UMS lookups', 1
EXEC [dbo].pInsTranslation 'Could_not_create_a_new_Contract_Type', N'غير قادر على إنشاء نوع عقد جديد.', 'Unable to create a new contract type.', 'UMS lookups', 1
EXEC [dbo].pInsTranslation 'Edit_Ep_Contract_Type', N'تحرير نوع عقد EP', 'Edit EP Contract Type', 'UMS lookups', 1
EXEC [dbo].pInsTranslation 'Event_Name', N'اسم الحدث', 'Event Name', 'Notification Event', 1
EXEC [dbo].pInsTranslation 'Update_Web_systems', N'تحديث أنظمة الويب', 'Update Web systems', 'Notification Event', 1
EXEC [dbo].pInsTranslation 'Update_Group_Type', N'تحديث نوع المجموعة', 'Update Group Type', 'Group access type', 1
EXEC [dbo].pInsTranslation 'User_Type', N'نوع المستخدم', 'User Type', 'Group access type', 1
EXEC [dbo].pInsTranslation 'Kuwait_AlYawm_Document', N'وثيقة الكويت اليوم', 'Kuwait Al Yawm Document', 'Legislation Document List', 1

EXEC [dbo].pInsTranslation 'Designation_Added_Successfully',N'لقد تمت إضافة التسمية بنجاح.','The designation has been added successfully.','UMS Lookups',1 
EXEC [dbo].pInsTranslation 'Grade_Type_Added_Successfully',N'تمت إضافة نوع الدرجة بنجاح.','The grade type has been added successfully.','UMS Lookups',1 
EXEC [dbo].pInsTranslation 'Grade_Type_Updated_Successfully',N'تم تحديث نوع الدرجة بنجاح.','The grade type has been updated successfully.','UMS Lookups',1 
EXEC [dbo].pInsTranslation 'Grade_Type_could_not_be_updated',N'لم يتم تحديث نوع الدرجة.','The grade type has not been updated.','UMS Lookups',1 
EXEC [dbo].pInsTranslation 'EP_Grade_Type',N'نوع درجة EP','EP Grade Type','UMS Lookups',1 
EXEC [dbo].pInsTranslation 'Edit_Ep_Grade_Type',N'تعديل نوع درجة EP','Edit EP Grade Type','UMS Lookups',1 
EXEC [dbo].pInsTranslation 'Add_Ep_Grade_Type',N'إضافة نوع درجة EP','Add EP Grade Type','UMS Lookups',1 
EXEC [dbo].pInsTranslation 'Could_not_create_a_new_Grade_Type',N'غير قادر على إنشاء نوع درجة جديد.','Unable to create a new grade type.','UMS Lookups',1 
EXEC [dbo].pInsTranslation 'Are_you_Sure_You_Want_to_Delete_Court_Type',N'هل أنت متأكد أنك تريد حذف نوع المحكمة؟','Are you sure you want to delete court type?','Court Detail',1 
EXEC [dbo].pInsTranslation 'Shift_Name',N'يحول','Shift','Court Detail',1 
EXEC [dbo].pInsTranslation 'Courts_History',N'المحاكم التاريخ','Courts History','GE Lookups',1 
EXEC [dbo].pInsTranslation 'Chambers_History',N'تاريخ الغرف','Chambers History','GE Lookups',1 
EXEC [dbo].pInsTranslation 'Month',N'شهر','Month','Case File Number Pattern',1 
EXEC [dbo].pInsTranslation 'Day',N'يوم','Day','Training Screen',1 
EXEC [dbo].pInsTranslation 'SubType',N'النوع الفرعي','Sub Type','Task Detail Screen',1 
EXEC [dbo].pInsTranslation 'Sub_Type',N'النوع الفرعي','Sub Type','Task Detail Screen',1
EXEC [dbo].pInsTranslation 'Default_Receiver',N'جهاز الاستقبال الافتراضي','Default Receiver','Sector Type',1 
EXEC [dbo].pInsTranslation 'Nationality_Name',N'الجنسيات','Nationalities','UMS Lookups',1 
EXEC [dbo].pInsTranslation 'Are_you_Sure_You_Want_to_Delete_Department',N'هل أنت متأكد أنك تريد حذف القسم؟','Are you sure you want to delete department?','UMS Lookups',1 

EXEC [dbo].pInsTranslation 'Add_Case_Party',N'إضافة جهة للقضية','Add Case Party','Case File',1

EXEC [dbo].pInsTranslation 'Add_Bank_Detail',N'إضافة تفاصيل البنك','Add Bank Detail','Add Bank Detail',1 
EXEC [dbo].pInsTranslation 'Bank_Name',N'اسم البنك','Bank Name','Add Bank Detail',1 
EXEC [dbo].pInsTranslation 'IBAN_Number',N'رقم IBAN','IBAN Number','Add Bank Detail',1 
EXEC [dbo].pInsTranslation 'Are_you_Sure_You_Want_to_Delete_Record',N'هل أنت متأكد أنك تريد حذف السجل؟','Are you sure you want to delete record?','Add Bank Detail',1 
EXEC [dbo].pInsTranslation 'Government_Entity_Representative_Updated_Successfully',N'تم تحديث ممثل الكيان الحكومي بنجاح.','Government entity representative updated successfully.','Common Lookups',1 
EXEC [dbo].pInsTranslation 'This_Sequence_Number_Already_Selected',N'تم تحديد هذا الرقم التسلسلي بالفعل.','This sequence number already selected.','Number Pattern',1 
EXEC [dbo].pInsTranslation 'Sure_Submit_Request',N'هل أنت متأكد أنك تريد تقديم الطلب؟','Are you sure you want to submit request?','Case File',1 
EXEC [dbo].pInsTranslation 'Department_History',N'تاريخ القسم','Department History','lookups History list',1 
EXEC [dbo].pInsTranslation 'Edit_Ep_Designation',N'تحرير تعيين Ep','Edit Ep Designation','Nationality',1 
EXEC [dbo].pInsTranslation 'Designation_Updated_Successfully',N'تم تحديث التسمية بنجاح','Designation Updated Successfully','Nationality',1 
EXEC [dbo].pInsTranslation 'Are_you_Sure_You_Want_to_Delete_Literature_Tag',N'هل أنت متأكد أنك تريد حذف علامة الأدب؟','Are you sure you want to delete literature tag?','Tag No',1
EXEC [dbo].pInsTranslation 'Book_Author_Added_Successfully',N'لقد تمت إضافة مؤلف الكتاب بنجاح.','The book author has been added successfully.','Book Author',1 


-----21-04-2025---
 
 EXEC [dbo].pInsTranslation 'Arabic_Name_Already_Exist',N'الإسم العربي موجود حاليا','Arabic Name Already Exist','Common',1
EXEC [dbo].pInsTranslation 'Create_Administartive_Case_File',N'إنشاء ملف قضية إدارية','Create Administartive Case File','create casefile',1
EXEC [dbo].pInsTranslation 'Both_English_And_Arabic_Names_Already_Exist',N'الأسماء العربية والإنجليزية موجودة حاليا','Both English And Arabic Names Already Exist','Common',1
 EXEC [dbo].pInsTranslation 'Create_CivilCommercial_Case_File',N'إنشاء ملف قضية مدنية / تجارية','Create Civil Commercial Case File','create casefile',1

EXEC [dbo].pInsTranslation 'Publication_Date_From', N'تاريخ النشر من', 'Publication Date From', 'Legal Legislation List', 1
EXEC [dbo].pInsTranslation 'Publication_Date_To', N'تاريخ النشر إلى', 'Publication Date To', 'Legal Legislation List', 1

EXEC [dbo].pInsTranslation 'AD_UserName', N'اسم المستخدم', 'AD Username', 'Legal Legislation List', 1
EXEC [dbo].pInsTranslation 'Under_Filing_Case_Section', N'قضايا تحت الرفع', 'Underfilling Cases Section', 'Principle Detail Requests page', 1
EXEC [dbo].pInsTranslation 'Time_Log_Under_Filing', N'سجل الوقت تحت الرفع', 'Time Log Underfilling', 'Time Log', 1
EXEC [dbo].pInsTranslation 'Under_Filing', N'تحت الرفع', 'Underfiling', 'Case File', 1
EXEC [dbo].pInsTranslation 'Assign_For_Filling', N'تم تعيين الطلب لقطاع تحت الرفع', 'Request has been assigned for Underfiling Sector', 'Case File', 1

---23/06/2025 by Danish----------
EXEC [dbo].pInsTranslation 'Total_Tickets',N'إجمالي المشاكل','Total Tickets','General',1
EXEC [dbo].pInsTranslation 'Pending_Tickets',N'المشاكل المتبقية','Pending Tickets','General',1
EXEC [dbo].pInsTranslation 'Completed_Tickets',N'المشاكل المنجزة','Completed Tickets','General',1
EXEC [dbo].pInsTranslation 'Party_Name',N'إسم المدعي/المدعي عليه','Party Name','General',1

---23/06/2025 by Danish---------
-------29/06/2025-------
EXEC [dbo].pInsTranslation 'New_Password', N'كلمة المرور الجديدة', 'New Password', 'Reset Password', 1

EXEC [dbo].pInsTranslation 'Reset_Password', N'إعادة تعيين كلمة المرور', 'Reset Password', 'Reset Password', 1


EXEC [dbo].pInsTranslation 'Temp_Password_Policy', N'يجب إيصال كلمة المرور المؤقتة إلى المستخدم المعني. يمكن تغيير كلمة المرور عند تسجيل الدخول.', 'The temporary password should be communicated to respective user. Using this password user can change password on login', 'Reset Password', 1

EXEC [dbo].pInsTranslation 'Minimum_8_Characters', N'الحد الأدنى 8 أحرف', 'Minimum 8 Characters', 'Reset Password', 1
EXEC [dbo].pInsTranslation 'Password_Policy', N'سياسة كلمة المرور', 'Password Policy', 'Reset Password', 1
EXEC [dbo].pInsTranslation 'Must_Have_8_Characters', N'يجب أن تكون على الأقل 8 أحرف', 'Must contain atleast 8 Charcter', 'Reset Password', 1
EXEC [dbo].pInsTranslation 'Must_Have_Atleast_1_Uppercase_Letter', N'يجب أن تحتوي على الأقل على حرف واحد كبير', ' Must contain atleast 1 uppercase letter', 'Reset Password', 1
EXEC [dbo].pInsTranslation 'Must_Have_Atleast_1_Lowerercase_Letter', N'يجب أن تحتوي على الأقل على حرف واحد صغير', 'Must contain atleast 1 lowercase letter', 'Reset Password', 1
EXEC [dbo].pInsTranslation 'Must_Have_Atleast_1_Number', N'يجب أن تحتوي على الأقل على رقم واحد', 'Must contain atleast 1 number', 'Reset Password', 1
EXEC [dbo].pInsTranslation 'Must_Have_Atleast_1_Special_Character', N'يجب أن تحتوي على الأقل على رمز واحد', 'Must contain at least 1 special character', 'Reset Password', 1
EXEC [dbo].pInsTranslation 'Confirm_Password', N'تأكيد كلمة المرور', 'Confirm Password', 'Reset Password', 1
EXEC [dbo].pInsTranslation 'Require_Valid_Password', N'يجب ادخال كلمة مرور صحيحة', 'Required Valid Password', 'Reset Password', 1

EXEC [dbo].pInsTranslation 'Please_Add_Filter', N'يرجى إدخال قيمة تصفية واحدة على الأقل', 'Please enter a value for at least one filter', 'General', 1
EXEC [dbo].pInsTranslation 'Custome_Rolls', N'طلبات الرول المخصصة', 'MOJ Custom Rolls', 'Moj Custom Rolls List', 1


-----------------------------------

Update tTranslation set Value_Ar = N'إذا كانت هذه التذكرة مرتبطة بأي خلل، يرجى تحديد الخلل' , Value_En = 'If this ticket is related to any bug, please select the bug.' where TranslationKey = 'Ticket_Related_To_Bug'
Update tTranslation set Value_Ar = N'المشاكل ذات الصلة' where TranslationKey = 'Related_Bugs'
EXEC [dbo].pInsTranslation 'Items_Ielected', N'عناصر محددة', 'items selected', 'Generic RZ-DropDown', 1
EXEC [dbo].pInsTranslation 'Goverment_Entity', N'الجهة الحكومية', 'Government Entity', 'G2G Request Detail  ', 1
EXEC [dbo].pInsTranslation 'Legal_Notification_Reply', N'رد بإخطار قانوني', 'Legal