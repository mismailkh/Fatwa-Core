
-- Notifications

EXEC [dbo].pInsTranslation 'Select_User',N'الرجاء تحديد مستخدم!','Please Select a User!','General',1  
EXEC [dbo].pInsTranslation 'Borrower_Add_Success',N'تم إضافة المستعير بنجاح	','Borrower successfully added','General',1
EXEC [dbo].pInsTranslation 'Unable_to_add_borrower!',N'غير قادر على إضافة المستعير!','Unable to add borrower!','General',1 
EXEC [dbo].pInsTranslation 'Unable_to_create_new_literature_category!',N'تعذر إنشاء تصنيف أدب جديد!','Unable to create new literature category!','General',1
  
EXEC [dbo].pInsTranslation 'The_barcode_has_been_printed_successfully',N'.تمت طباعة الرمز الشريطي بنجاح','The barcode has been printed successfully','General',1 
EXEC [dbo].pInsTranslation 'Something_went_wrong_Please_try_again',N'لقد حدث خطأ ما. يرجى المحاولة مرة أخرى لاحقا','Something went wrong. Please try again','General',1
EXEC [dbo].pInsTranslation 'Poster_printed_successfully',N'.تمت طباعة الملصق بنجاح','Poster printed successfully','Edit Lms Literature Detail page',1
EXEC [dbo].pInsTranslation 'Literature_Delete_Failed',N'خطأ في تنفيذ عملية الحذف.','Error executing the delete operation.','General',1
EXEC [dbo].pInsTranslation 'Sure_Delete_Literature',N'هل أنت متأكد أنك تريد الحذف؟','Are you sure you want to delete?','General',1
EXEC [dbo].pInsTranslation 'Literature_Delete_Success',N'تمت إزالة الأدبيات المختارة بنجاح من النظام.','The selected Literature(s) have been successfully removed from the system.','General',1
EXEC [dbo].pInsTranslation 'Select_Record_For_Delete',N'يرجى تحديد السجلات المراد حذفها!','Please select the records to be deleted!','General',1 
 
EXEC [dbo].pInsTranslation 'Index_number_is_already_registered_Please_enter_another_number',N'رقم الفهرس مسجل بالفعل الرجاء إدخال رقم آخر','Index number is already registered. Please enter another index number','General',1
EXEC [dbo].pInsTranslation 'Could_not_create_a_new_literature',N'تعذر إنشاء نوع أدب جديد!','Could not create a new literature','General',1
EXEC [dbo].pInsTranslation 'Literature_Index_Create_Success',N'تم إنشاء الفهرس بنجاح','Index successfully created','General',1
EXEC [dbo].pInsTranslation 'Literature_Parent_Index_Create_Success',N'Parent Index successfully created','Parent Index successfully created','General',1
EXEC [dbo].pInsTranslation 'Changes_saved_successfully',N'.االتغييرات التي تم حفظها بنجاح','Changes saved successfully','General',1
EXEC [dbo].pInsTranslation 'Literature_Index_Delete_Success',N'تم حذف الفهرس بنجاح','Index has been deleted successfully','General',1
 
EXEC [dbo].pInscTranslation 'Borrower_could_not_be_updated',N'تعذر تحديث المستعير!','Borrower Could Not Be Updated!','General',1
EXEC [dbo].pInsTranslation 'Borrower_updated_successfully',N'تم تحديث المستعير بنجاح!','Borrower Updated Successfully!','General',1
EXEC [dbo].pInsTranslation 'Error_loading_borrower_details',N'خطأ في تحميل تفاصيل المستعير','Edit_Lms_Literature_Borrow_Detail','General',1 
EXEC [dbo].pInsTranslation 'Are_you_sure_you_ want_to_save_this_change',N'هل أنت متأكد أنك تريد حفظ هذا التغيير؟','Are you sure you want to save this change','General',1
 
EXEC [dbo].pInsTranslation 'The_poster_has_been_modified_Please_print_again_and_attach_to_your_book_record',N'.تم تعديل الملصق. يرجى الطباعة مرة أخرى وإرفاقها بسجل الكتاب.','The poster has been modified. Please print again and attach to your book record.','General',1
  
EXEC [dbo].pInsTranslation 'Literature_type_could_not_be_updated',N'.تعذر تحديث نوع الكتاب','Literature type could not be updated','General',1
EXEC [dbo].pInsTranslation 'Literatue_Index_Division_Delete_Failed',N'تعذر حذف الأدب','Literature could not be deleted','General',1
EXEC [dbo].pInsTranslation 'Literatue_Index_Add_Success',N'أضف فهرس الأدب بنجاح','Literature Index Added Successfully','General',1 
 
EXEC [dbo].pInsTranslation 'Approve_Literature_Extension',N'هل أنت متأكد أنك تريد الموافقة على طلب التمديد؟	','Are you sure you want to approve the extension request?','General',1 
EXEC [dbo].pInsTranslation 'Approve_Literature_Extension_Success',N'تمت الموافقة على طلب التمديد بنجاح.','The extension request has been successfully approved.','General',1 
EXEC [dbo].pInsTranslation 'Reject_Literature_Extension',N'هل أنت متأكد أنك تريد رفض طلب التمديد؟','Are you sure you want to reject the extension request?','General',1 
EXEC [dbo].pInsTranslation 'Reject_Literature_Extension_Success',N'تم بنجاح رفض طلب تمديد فترة الاستعارة.','The request to extend the borrowing period was successfully rejected.','General',1 
EXEC [dbo].pInsTranslation 'Literature_Extension_Error',N'تعذر تحديث طلب الموافقة على الاستعارة!','The borrow approval request could not be updated!','General',1 
EXEC [dbo].pInsTranslation 'Literature_Extension_Load_Error',N'خطأ في تحميل تفاصيل المستعير	','Error loading the borrower details','General',1 
 
EXEC [dbo].pInsTranslation 'Fill_Required_Fields',N'يرجى ملء جميع الحقول المطلوبة','Please fill all the required fields.','General',1
EXEC [dbo].pInsTranslation 'Print_All',N'طباعة الكل','Print All','Literature',1
 
EXEC [dbo].pInsTranslation 'Approve_Document_Draft',N'هل أنت متأكد أنك تريد الموافقة على المسودة؟','Are you sure you want to Approve Draft?','General',1 
EXEC [dbo].pInsTranslation 'Approve_Document_Draft_Success',N'تمت الموافقة على المسودة بنجاح.','The Draft is Successfully Approved.','General',1 
EXEC [dbo].pInsTranslation 'Reject_Document_Draft',N'هل أنت متأكد أنك تريد رفض المسودة؟','Are you sure you want to Reject Draft?','General',1 
EXEC [dbo].pInsTranslation 'Reject_Document_Draft_Success',N'تم رفض المسودة بنجاح.','The Draft is Successfully Rejected.','General',1 
EXEC [dbo].pInsTranslation 'Sync_document',N'Sync document','Sync document','General',1 

EXEC [dbo].pInsTranslation 'Modify_Document_Draft',N'هل أنت متأكد أنك تريد تعديل المسودة؟','Are you sure you want to modify the Draft?','General',1 
EXEC [dbo].pInsTranslation 'Modify_Document_Draft_Success',N'تم إرسال المسودة بنجاح لمزيد من التعديل.','Draft is successfully sent back for further modification.','General',1 
EXEC [dbo].pInsTranslation 'Send_Comment_Document_Draft',N'هل أنت متأكد أنك تريد إرسال تعليق؟','Are you sure you want to Send a Comment?','General',1 
EXEC [dbo].pInsTranslation 'Send_Comment_Document_Draft_Success',N'تم حفظ المسودة بنجاح مع التعليق للمراجعة.','Draft is successfully Saved with comment for review.','General',1 
EXEC [dbo].pInsTranslation 'Sure_Cancel',N'هل أنت متأكد أنك تريد الإلغاء؟','Are you sure you want to cancel?','General',1 
EXEC [dbo].pInsTranslation 'Contact_Administrator',N'لقد حدث خطأ ما. رجاء تواصل مع مسؤول النظام.','Something is wrong, Please contact the Administrator','General',1 
EXEC [dbo].pInsTranslation 'Error_Load_ApprovalDetail',N'خطأ في تحميل تفاصيل الموافقة على المستند','Error loading the Document Approval details','General',1 
EXEC [dbo].pInsTranslation 'Item_Unavailable',N'لم يعد العنصر متاحًا','Item No Longer Available','General',1 
EXEC [dbo].pInsTranslation 'Delete_Success',N'تم حذف السجل بنجاح','Record is Deleted Successfully','Lms Literature BorrowDetail page',1

EXEC [dbo].pInsTranslation 'FromDate_NotGreater_ToDate',N'From Date Should not be Greater Than To Date','From Date Should not be Greater Than To Date','Lms Literature page',1

--MainLayout
EXEC [dbo].pInsTranslation 'Fatwa_Header',N'فتوى','FATWA','Header Title',1

--index page  
EXEC [dbo].pInsTranslation 'Home',N'الصفحة الرئيسية','Home','General',1
EXEC [dbo].pInsTranslation 'Dashboard',N'لوحة القيادة','Dashboard','Indexpage',1
EXEC [dbo].pInsTranslation 'Investigation',N'التحقيق','Investigation','Indexpage',1
EXEC [dbo].pInsTranslation 'Welcome_to_your_new_app',N'مرحبًا بك في تطبيقك الجديد','Welcome to your new app','Indexpage',1
EXEC [dbo].pInsTranslation 'Welcome_to_Fatwa',N'مرحبا بكم في فتوى','Welcome to Fatwa','Indexpage',1
EXEC [dbo].pInsTranslation 'index',N'فهرس','Dashboard','Indexpage',1

EXEC [dbo].pInsTranslation 'Index_of_the_most_important_borrowed_books',N'فهرسأهم الكتب المستعارة','Index of the most important borrowed books','Indexpage',1
EXEC [dbo].pInsTranslation 'Rate_of_Return',N'معدل العائد','Rate of Return','Indexpage',1
EXEC [dbo].pInsTranslation 'Most_popular_books',N'أهم الكتب المستعارة','Most popular books','Indexpage',1
EXEC [dbo].pInsTranslation 'solved',N'تم الحل','solved','Indexpage',1
EXEC [dbo].pInsTranslation 'replied',N'أجاب','replied','Indexpage',1
EXEC [dbo].pInsTranslation 'A_question',N'سؤال','A question','Indexpage',1
EXEC [dbo].pInsTranslation 'Problems',N'مشاكل','A question','Indexpage',1
EXEC [dbo].pInsTranslation 'Problems',N'مشاكل','A question','Indexpage',1
EXEC [dbo].pInsTranslation 'library_management_system',N'النظام الإداري','Management System','Indexpage',1
EXEC [dbo].pInsTranslation 'Total_books',N'مجموع الكتب','Total books','Indexpage',1
EXEC [dbo].pInsTranslation 'late_borrowing',N'الاقتراض المتأخر','Late borrowing','Indexpage',1
 
EXEC [dbo].pInsTranslation 'mistakes',N'أخطاء','mistakes','Indexpage',1
EXEC [dbo].pInsTranslation 'Ahmad',N'احمد','Ahmad','Indexpage',1
EXEC [dbo].pInsTranslation 'Omar',N'عمر','Omar','Indexpage',1
EXEC [dbo].pInsTranslation 'Aqeel',N'عقيل','Aqeel','Indexpage',1
EXEC [dbo].pInsTranslation 'Hassan',N'حسن','Hassan','Indexpage',1
EXEC [dbo].pInsTranslation 'zain',N'زين','zain','Indexpage',1

EXEC [dbo].pInsTranslation 'kuram',N'خرم','kuram','Indexpage',1
EXEC [dbo].pInsTranslation 'Arslan',N'ارسلان','Arslan','Indexpage',1
EXEC [dbo].pInsTranslation 'Dashboard',N'لوحة القيادة','Dashboard','Indexpage',1

--Login 
EXEC [dbo].pInsTranslation 'Council_of_Ministers',N'مجلس الوزراء','Council of Ministers','loginpage',1
EXEC [dbo].pInsTranslation 'Fatwas_and_Legislation',N'الفتوع و اللتشريع','Fatwas and Legislation','loginpage',1
EXEC [dbo].pInsTranslation 'email',N'بريد الالكتروني','Email','loginpage',1
EXEC [dbo].pInsTranslation 'password',N'كلمة المرور','password','loginpage',1
EXEC [dbo].pInsTranslation 'Fatwas_and_Legislation',N'الفتوع و اللتشريع','Fatwas and Legislation','loginpage',1
EXEC [dbo].pInsTranslation 'Incorrect_email_or_password!',N'بريد أو كلمة مرورغير صحيحة!','Incorrect email or password!','loginpage',1
EXEC [dbo].pInsTranslation 'welcome_back!_Login_to_your_account.',N'مرحبا بعودتك! تسجيل الدخول إلى حسابك.','welcome back! Login to your account.','loginpage',1
EXEC [dbo].pInsTranslation 'sign_in',N'تسجيل الدخول','sign in','loginpage',1
 
EXEC [dbo].pInsTranslation 'Are_you_sure_you_want_to_get_rid?',N' هل أنت متأكد أنك تريد التخلص؟','Are you sure you want to get rid?','loginpage',1
EXEC [dbo].pInsTranslation 'Loading',N'جاري تحميل','Loading','loginpage',1
EXEC [dbo].pInsTranslation 'Loading',N'جاري تحميل','Loading','loginpage',1
--Role list Page 
EXEC [dbo].pInsTranslation 'Role_List',N'قائمة الأدوار','Role List','RoleListPage ',1
EXEC [dbo].pInsTranslation 'Add_a_role',N'أضف دورًا','Add a role','RoleListPage',1

EXEC [dbo].pInsTranslation 'ID',N'هوية شخصية','ID','RoleListPage',1
EXEC [dbo].pInsTranslation 'Name',N'الاسم','Name','RoleListPage',1
EXEC [dbo].pInsTranslation 'natural_name',N'الاسم الطبيعي','natural name','RoleListPage',1
EXEC [dbo].pInsTranslation 'procedures',N'أجراءات','procedures','RoleListPage',1
EXEC [dbo].pInsTranslation 'release',N'يحرر','release','RoleListPage',1
EXEC [dbo].pInsTranslation 'delete',N'حذف','Delete','RoleListPage',1

EXEC [dbo].pInsTranslation 'Permission_management',N'إدارة الأذونات','Permission management','RoleListPage',1
EXEC [dbo].pInsTranslation 'Memorizes',N'يحفظ','Memorizes','RoleListPage',1 
-- User list
EXEC [dbo].pInsTranslation 'User_List',N'قائمة المستخدم','User List','RoleListPage',1 
 EXEC [dbo].pInsTranslation 'Borrower_Full_Name',N'الاسم الكامل للمقترض','Borrower Full Name','loginpage',1
 EXEC [dbo].pInsTranslation 'User_Full_Name',N'الاسم الكامل للمستخدم','User Full Name','loginpage',1
     
EXEC [dbo].pInsTranslation 'Export_all_pages',N'تصدير كافة الصفحات','Export_all_pages','loginpage',1
EXEC [dbo].pInsTranslation 'user_name',N'اسم المستخدم','User Name','loginpage',1
EXEC [dbo].pInsTranslation 'Email_address',N'عنوان بريد الكتروني','Email address','loginpage',1
EXEC [dbo].pInsTranslation 'procedures',N'أجراءات','procedures','loginpage',1

--Processlogs page  
EXEC [dbo].pInsTranslation 'Operation_logs',N'سجلات العمليات','Process Logs','Process Logs page',1
EXEC [dbo].pInsTranslation 'Download',N'تحميل','Download','Processlogs page',1
EXEC [dbo].pInsTranslation 'Search',N'بحث','Search','Generic Search',1
EXEC [dbo].pInsTranslation 'Process',N'معالجة','Process','Processlogs page',1
EXEC [dbo].pInsTranslation 'Task',N'مهمة','Task','Processlogs page',1
EXEC [dbo].pInsTranslation 'starting_date',N'تاريخ البدء','Starting Date','Processlogs page',1
EXEC [dbo].pInsTranslation 'Process_Logs',N'سجلات العمليات','Process Logs','Processlogs page',1
EXEC [dbo].pInsTranslation 'borrowed',N'مستعار','Borrowed','Processlogs page',1
EXEC [dbo].pInsTranslation 'There_are_no_records!',N'لا يوجد سجلات!','There are no records!','Processlogs page',1
EXEC [dbo].pInsTranslation 'Book_Name',N'اسم الكتاب','Book Name','Processlogs page',1

EXEC [dbo].pInsTranslation 'Expiry_date',N'تاريخ الانتهاء','Expiry date','Processlogs page',1

EXEC [dbo].pInsTranslation 'Description',N'الوصف','Description','Processlogs page',1
EXEC [dbo].pInsTranslation 'Description_En_Doc',N'وصف (انجليزي)','Description (English)','Processlogs page',1

EXEC [dbo].pInsTranslation 'computer',N'الحاسوب','Computer','Processlogs page',1
EXEC [dbo].pInsTranslation 'Data_recording',N'تسجيل البيانات','Data Recording','Processlogs page',1
EXEC [dbo].pInsTranslation 'Literary_Borrow_Edit',N'تعديل طلب الاستعارة','Edit Borrow Request','Literature Borrow Approval page',1 

-- AddLmsLiteratureBorrowDetail page 
EXEC [dbo].pInsTranslation 'Add_literature_Barrow_details',N'إضافة تفاصيل استعارة الكتاب','Add Literature Borrow Details','AddLmsLiteratureBorrowDetail page',1
EXEC [dbo].pInsTranslation 'Add_literature_details',N'أضف استعارة الأدب','Add literature details','AddLmsLiteratureBorrowDetail page',1
EXEC [dbo].pInsTranslation 'Add_literature_metaphor_details',N'إضافة تفاصيل استعارة الأدب','Add literature metaphor details','AddLmsLiteratureBorrowDetail page',1
EXEC [dbo].pInsTranslation 'User_information',N'معلومات المستخدم','User information','AddLmsLiteratureBorrowDetail page',1 
EXEC [dbo].pInsTranslation 'Borrower_Name',N'اسم المستعير','Borrower Name','AddLmsLiteratureBorrowDetail page',1 
EXEC [dbo].pInsTranslation 'Username_is_required',N'اسم المستخدم مطلوب','Username is required','AddLmsLiteratureBorrowDetail page',1
EXEC [dbo].pInsTranslation 'Mobile_Phone_Number',N'رقم الهاتف النقال','Mobile Number','AddLmsLiteratureBorrowDetail page',1
EXEC [dbo].pInsTranslation 'Book_borrow_information',N'تفاصيل الكتاب','Book Detail','AddLmsLiteratureBorrowDetail page',1
EXEC [dbo].pInsTranslation 'literatureSideMenu',N'المكتبة القانونية','Legal Literature','AddLmsLiteratureBorrowDetail page',1
EXEC [dbo].pInsTranslation 'literature_Heading',N'الكتب','Literatures','AddLmsLiteratureBorrowDetail page',1
EXEC [dbo].pInsTranslation 'ISBN',N'الرقم العالمي الموحد للكتاب','ISBN Number','AddLmsLiteratureBorrowDetail page',1
EXEC [dbo].pInsTranslation 'Issue_Date',N'تاريخ بداية الاستعارة	','Issue Date','AddLmsLiteratureBorrowDetail page',1
EXEC [dbo].pInsTranslation 'Issue_date_required',N'تاريخ الإصدار مطلوب"','Issue date required','AddLmsLiteratureBorrowDetail page',1 
EXEC [dbo].pInsTranslation 'extended',N'تم التمديد','Extended','AddLmsLiteratureBorrowDetail page',1
EXEC [dbo].pInsTranslation 'Number_of_borrowed_books',N'عدد الكتب المستعارة','Number of borrowed books','AddLmsLiteratureBorrowDetail page',1
EXEC [dbo].pInsTranslation 'Save',N'حفظ','Save','AddLmsLiteratureBorrowDetail page',1
EXEC [dbo].pInsTranslation 'The_number_of_books_borrowed_must_be_between_0_and_3',N'يجب أن يكون عدد الكتب المستعارة بين 0 و 3','The number of books borrowed must be between 0 and 3','AddLmsLiteratureBorrowDetail page',1
EXEC [dbo].pInsTranslation 'Authors_name_is_required',N'مطلوب اسم المؤلفات','Authors name is required','AddLmsLiteratureBorrowDetail page',1
EXEC [dbo].pInsTranslation 'warning',N'تحذير','warning','AddLmsLiteratureBorrowDetail page',1
EXEC [dbo].pInsTranslation 'Please_select_literature',N'الرجاء اختيار الكتاب','Please Select Literature','AddLmsLiteratureBorrowDetail page',1
EXEC [dbo].pInsTranslation 'Literature_is_required',N'الأدب مطلوب','Literature is Required','AddLmsLiteratureBorrowDetail page',1 
EXEC [dbo].pInsTranslation 'Are_you_sure_you_want_to_add_this_borrower',N'هل أنت متأكد أنك تريد إضافة هذا المستعير','Are you sure you want to add this borrower','AddLmsLiteratureBorrowDetail page',1
EXEC [dbo].pInsTranslation 'OK',N'موافق','OK','AddLmsLiteratureBorrowDetail page',1
EXEC [dbo].pInsTranslation 'Sure',N'نجاح','Sure','AddLmsLiteratureBorrowDetail page',1
EXEC [dbo].pInsTranslation 'success',N'يتأكد','success','AddLmsLiteratureBorrowDetail page',1
EXEC [dbo].pInsTranslation 'Borrow_Another_Book',N'هل تريد استعارة كتاب آخر؟','Would you like to borrow another book?','AddLmsLiteratureBorrowDetail page',1 
EXEC [dbo].pInsTranslation 'Error',N'خطأ','Error','AddLmsLiteratureBorrowDetail page',1
EXEC [dbo].pInsTranslation 'Sure',N'يتأكد','Sure','AddLmsLiteratureBorrowDetail page',1
EXEC [dbo].pInsTranslation 'Add_Borrower',N'إضافة المستعير','Add Borrower','AddLmsLiteratureBorrowDetail page',1
EXEC [dbo].pInsTranslation 'Extend_Due_Date',N'تاريخ انتهاء فترة التمديد','Extended Due Date','AddLmsLiteratureBorrowDetail page',1
EXEC [dbo].pInsTranslation 'Borrow_Book_Limit',N'قام المستخدم باستعارة 3 كتب','User Already Borrowed 3 Books','AddLmsLiteratureBorrowDetail page',1
EXEC [dbo].pInsTranslation 'Search_Text',N'بحث نصي','Search Text','AddLmsLiteratureBorrowDetail page',1


--Side bar menus
EXEC [dbo].pInsTranslation 'Dashboard',N'لوحة التحكم','Dashboard','Dashboard page',1 
EXEC [dbo].pInsTranslation 'Literature_Parent_Index',N'فهرس الكتاب','Book Parent Index','Side Bar menu page',1 
EXEC [dbo].pInsTranslation 'Literature_Index',N'فهرس الكتب','Book Index','Side Bar menu page',1 
EXEC [dbo].pInsTranslation 'Literature_Type',N'نوع الكتاب','Literature Type','Side Bar menu page',1
EXEC [dbo].pInsTranslation 'Literary_borrowing_details',N'تفاصيل الاستعارة الأدبية','Literary borrowing details','Side Bar menu page',1
EXEC [dbo].pInsTranslation 'Literature_Classification',N'تصنيف الكتاب','Literature Classification','Side Bar menu page',1
EXEC [dbo].pInsTranslation 'Logs',N'السجلات','Logs','Side Bar menu page',1
EXEC [dbo].pInsTranslation 'ErrorLogs',N' سجل الخطأ','Error Logs','Side Bar menu page',1
EXEC [dbo].pInsTranslation 'Multi_Level',N'سجلات متعدد ','Multi level','Side Bar menu page',1 
EXEC [dbo].pInsTranslation 'Agreeing_to_extend_the_borrowing_of_books',N'تمديد فترة استعارة الكتب','Literatures Borrow Extension','Side Bar menu page',1
EXEC [dbo].pInsTranslation 'LogOut',N'تسجيل خروج','LogOut','Side Bar menu page',1
EXEC [dbo].pInsTranslation 'Legal_Documents',N'وثائق قانونية','Legal Documents','Side Bar menu page',1
EXEC [dbo].pInsTranslation 'Legal_Documents_Heading',N'الوثائق القانونية','Legal Documents','Side Bar menu page',1
EXEC [dbo].pInsTranslation 'Document_Catalogs',N'فهرس الوثائق','Documents Catalogue','Side Bar menu page',1
EXEC [dbo].pInsTranslation 'Legal_Document_Approval',N'مراجعة الوثائق','Review Documents','Side Bar menu page',1
EXEC [dbo].pInsTranslation 'Legal_Document_Masked',N'الوثائق المظللة','Masked Documents ','Side Bar menu page',1
 
--AddLmsLiteratureClassification page  
EXEC [dbo].pInsTranslation 'Add_category_literature',N'تصنيف الكتاب','Add literature Classification','Add Lms Literature Classification page ',1
EXEC [dbo].pInsTranslation 'Name_is_required',N'مطلوب اسم','Name is required','Add Lms Literature Classification page ',1
EXEC [dbo].pInsTranslation 'Classification_Name',N'الاسم (انجليزي)','Name','Lms Literature Classification page ',1
EXEC [dbo].pInsTranslation 'Classification_Name_Ar',N'الاسم (عربي)','Name (Arabic)','Lms Literature Classification page ',1 
EXEC [dbo].pInsTranslation 'Add_Classification',N'Add Classification','Add Classification','Lms Literature Classification page ',1 
--AddLmsLiteratureDetail page 
EXEC [dbo].pInsTranslation 'Add_Literature',N'أضف الأدب','Add Literature','Add Lms Literature Classification page ',1
EXEC [dbo].pInsTranslation 'Basic_details',N'فهرس','Literature Index','Add Lms Literature Classification page ',1
EXEC [dbo].pInsTranslation 'Select_Index',N'حدد الفهرس','Select Index','Add Lms Literature Classification page ',1
EXEC [dbo].pInsTranslation 'Select_Type',N'حدد نوع الكتاب','Select Type','Add Lms Literature Classification page ',1
EXEC [dbo].pInsTranslation 'Select_Classification',N'حدد التصنيف','Select Classification','Add Lms Literature Classification page ',1
EXEC [dbo].pInsTranslation 'Literature_Index',N'تصنيف الأدب ','Literatures Index','Add Lms Literature Classification page ',1
EXEC [dbo].pInsTranslation 'Purchase_Date',N'تاريخ الشراء','Purchase Date','Add Lms Literature Classification page ',1
EXEC [dbo].pInsTranslation 'Purchase_details',N'تفاصيل شراء','Purchase Details','Add Lms Literature Classification page ',1
EXEC [dbo].pInsTranslation 'Required_field',N'الكلمة الرئيسية مطلوبة','Keyword is required','Add Lms Literature Classification page ',1

  

 
-- LmsLiteratureIndex List page 
EXEC [dbo].pInsTranslation 'Add',N'إضافة الفهرس','Add Index','Lms Literature Index page ',1
EXEC [dbo].pInsTranslation 'Index_Number',N'رقم الفهرس','Index Number','Lms Literature Index page ',1 
EXEC [dbo].pInsTranslation 'Creation_Date',N'التاريخ','Date','Lms Literature Index page ',1 
EXEC [dbo].pInsTranslation 'Division_Index_Delete_Success',N'تم حذف رقم القسم والممر بنجاح.','Division and Aisle Number Deleted Successfully.','Lms Literature Index page ',1 
EXEC [dbo].pInsTranslation 'Division_Index_Delete_Fail',N'تعذر حذف رقم القسمة والممر.','Unable to Delete Division and Aisle number.','Lms Literature Index page ',1 
 
--AddLmsLiteratureIndex page 
EXEC [dbo].pInsTranslation 'Index_Name',N'اسم الفهرس','Index Name','Add Lms Literature LiteratureIndex page ',1
EXEC [dbo].pInsTranslation 'Index_Name_En',N'اسم الفهرس (إنجليزي)','Index Name (English)','Add Lms Literature LiteratureIndex page ',1
EXEC [dbo].pInsTranslation 'Index_Name_Ar',N'اسم الفهرس (عربي)','Index Name (Arabic)','Add Lms Literature LiteratureIndex page ',1
EXEC [dbo].pInsTranslation 'Add_Lms_Literature_Index',N'إضافة الفهرس','Add Index','Add Lms Literature LiteratureIndex page ',1
EXEC [dbo].pInsTranslation 'English_index_name_is_required',N'مطلوب اسم الفهرس إنجليزي','Index Name (English) is required','Add Lms LiteratureIndex page ',1
EXEC [dbo].pInsTranslation 'Index_Name_Range',N'يجب أن يكون اسم الفهرس بين 2 و 100','Index Name Must be Between 2 and 100','Add Lms Literature Index  page ',1 

EXEC [dbo].pInsTranslation 'Arabic_index_name_is_required',N'مطلوب إسم الفهرس عربي','Index Name (Arabic) is required','Add Lms Literature LiteratureIndex page ',1 
EXEC [dbo].pInsTranslation 'Index_number_is_required',N'رقم الفهرس مطلوب','Index number is required','Add Lms Literature LiteratureIndex page ',1
EXEC [dbo].pInsTranslation 'Index_Number_Range',N'Index number must be 3 digits','Index number must be 3 digits','Add Lms Literature Index  page ',1 
EXEC [dbo].pInsTranslation 'Information',N'معلومة','Information','Add Lms Literature LiteratureIndex page ',1
EXEC [dbo].pInsTranslation 'number_placeholder',N'أدخل رقم','Enter Number','Add Lms Literature LiteratureIndex page ',1
EXEC [dbo].pInsTranslation 'Number_range_2_and_100',N'يجب أن يكون الرقم بين 2 و 100','Number must be between 2 and 100','Add Lms Literature Index page',1
EXEC [dbo].pInsTranslation 'Index_Create',N'انشاء','Create','Add Lms Literature Index page',1

-- Lms Literature Parent Index Page
EXEC [dbo].pInsTranslation 'Could_not_create_a_new_index',N'تعذر إنشاء فهرس جديد','Could not create a new index','General',1





--Add Lms Literature Index division page  
EXEC [dbo].pInsTranslation 'Add_Lms_Literature_Index_Division',N'إضافة قسم فهرس الكتاب','Add Book Index Division','Add Lms Literature Index Division page ',1 
EXEC [dbo].pInsTranslation 'Division_Number_Required',N'رقم القسم مطلوب','Division Number Is Required','Add Lms Literature Index Division page ',1 
EXEC [dbo].pInsTranslation 'Division_Number_Range',N'يجب أن يكون رقم القسمة بين 2 و 100','Division Number Must be Between 2 and 100','Add Lms Literature Index Division page ',1 
EXEC [dbo].pInsTranslation 'Aisle_Number_Required',N'رقم الممر مطلوب','Aisle Number Is Required','Add Lms Literature Detail page',1
EXEC [dbo].pInsTranslation 'Aisle_Number_Range',N'يجب أن يكون رقم الممر بين 2 و 100','Aisle Number Must be Between 2 and 100','Add Lms Literature Index Division page ',1  
EXEC [dbo].pInsTranslation 'Section_created_successfully',N'تم إنشاء القسم بنجاح','Division successfully created','Add Lms Literature Index page ',1

--Edit Lms Literature Index division page  
EXEC [dbo].pInsTranslation 'Edit_Lms_Literature_Index_Division',N'تحرير قسم فهرس الكتاب','Edit Book Index Division','Edit Lms Literature Index Division page ',1

--Add Lms Literature Type page 
EXEC [dbo].pInsTranslation 'Add_Lms_Literature_Type',N'إضافة نوع الكتاب','Add Lms Literature Type','Add Lms Literature Type page',1
EXEC [dbo].pInsTranslation 'English_name',N'الاسم انجليزي',' Name (English)','Add Lms Literature Type page',1
EXEC [dbo].pInsTranslation 'English_name_required',N'مطلوب اسم انجليزي','English name required','Add Lms Literature Type page',1
EXEC [dbo].pInsTranslation 'Arabic_Name',N'الاسم بالعربية',' Name (Arabic)','Add Lms Literature Index page',1
EXEC [dbo].pInsTranslation 'Could_not_create_a_new_literature_type',N'تعذر إنشاء نوع أدب جديد','Could not create a new literature type','Add Lms Literature Type page',1
EXEC [dbo].pInsTranslation 'from_history',N'من التاريخ','From Date','Add Lms Literature Type page',1
EXEC [dbo].pInsTranslation 'Add_Type',N'إضافة النوع','Add Type','Add Lms Literature Type page',1

--Edit Lms LiteratureBorrowDetail page 
EXEC [dbo].pInsTranslation 'Edit_Lms_Literature_BorrowDetail',N'معلومة','Edit Lms Literature Borrow Detail','Edit Lms Literature Borrow Detail page',1
EXEC [dbo].pInsTranslation 'User_information',N'معلومات المستخدم','User information','Edit Lms Literature Borrow Detail page',1
EXEC [dbo].pInsTranslation 'Extension_of_the_due_date',N'تمديد تاريخ الاستحقاق','Extension of the due date','Edit Lms Literature Borrow Detail page',1
EXEC [dbo].pInsTranslation 'Return_date',N'تاريخ ارجاع الكتب','Return Date','Edit Lms Literature Borrow Detail page',1 




-- EditLmsLiteratureClassification page 
EXEC [dbo].pInsTranslation 'Literature_classification_editing',N'تحرير تصنيف الأدب','Edit Literature Classification ','Edit Lms Literature Classification page',1

--EditLmsLiteratureDetail page 
EXEC [dbo].pInsTranslation 'Literature_Editing',N'تحرير الأدب','Edit Literature ','Edit Lms Literature Detail page	',1
EXEC [dbo].pInsTranslation 'Literature_List',N'قائمة الأدب','Literature List','Edit Lms Literature Detail page',1
EXEC [dbo].pInsTranslation 'Basic_details',N'تفاصيل أساسية','Basic details','Edit Lms Literature Detail page',1
EXEC [dbo].pInsTranslation 'Division_Number',N'رقم القسم','Division Number','Edit Lms Literature Detail page',1
EXEC [dbo].pInsTranslation 'Aisle_Number',N'رقم الممر','Aisle Number','Edit Lms Literature Detail page',1
EXEC [dbo].pInsTranslation 'Subject_English',N'الموضوع (انجليزي)','Subject (English)','Edit Lms Literature Detail page',1
EXEC [dbo].pInsTranslation 'Subject_Arabic',N'الموضوع','Subject (Arabic)','Edit Lms Literature Detail page',1
EXEC [dbo].pInsTranslation 'Subject',N'الموضوع','Subject','Edit Lms Literature Detail page',1
EXEC [dbo].pInsTranslation 'Is_Series',N'هل لديه أجزاء؟','Is Series?','Edit Lms Literature Detail page',1
EXEC [dbo].pInsTranslation 'Series_Number',N'رقم الجزء','Series Number','Edit Lms Literature Detail page',1
EXEC [dbo].pInsTranslation 'Number_of_Pages',N'(300) عدد الصفحات','Number of Pages (300)','Edit Lms Literature Detail page',1
EXEC [dbo].pInsTranslation 'Number_of_Copies',N'عدد النسخ','Copy Count','Edit Lms Literature Detail page',1
EXEC [dbo].pInsTranslation 'location',N'مكان الشراء','Purchase Location','Edit Lms Literature Detail page',1
EXEC [dbo].pInsTranslation 'Price',N'السعر','Price','Edit Lms Literature Detail page',1
EXEC [dbo].pInsTranslation 'Purchase_Date',N'تاريخ الشراء','Purchase Date','Edit Lms Literature Detail page',1
EXEC [dbo].pInsTranslation 'Author_details',N'تفاصيل المؤلف','Author Details','Edit Lms Literature Detail page',1
EXEC [dbo].pInsTranslation 'Select_Author',N'حدد المؤلف','Select Author','Edit Lms Literature Detail page',1
EXEC [dbo].pInsTranslation 'Author_Associated_With',N'يرتبط هذا المؤلف','The selected author is associated with','Edit Lms Literature Detail page',1
EXEC [dbo].pInsTranslation 'Other_Literatures',N'الآداب الأخرى.','other Literatures.','Edit Lms Literature Detail page',1
EXEC [dbo].pInsTranslation 'Updated_author_details_will_be_reflected_across_all_linked_entries',N'ستنعكس تفاصيل المؤلف المحدثة عبر جميع الإدخالات المرتبطة.','Updated author details will be reflected across all associated entries.','Edit Lms Literature Detail page',1
EXEC [dbo].pInsTranslation 'Barcode',N'رقم الباركود','Barcode Number','Edit Lms Literature Detail page',1
EXEC [dbo].pInsTranslation 'Is_Borrowable',N'متاح للإعارة؟','Borrowable?','Edit Lms Literature Detail page',1
EXEC [dbo].pInsTranslation 'Is_Viewable',N'متاح للعرض؟','Viewable?','Edit Lms Literature Detail page',1
EXEC [dbo].pInsTranslation 'Allow_to_Publish',N'متاح للنشر؟','Allow to Publish?','Edit Lms Literature Detail page',1
EXEC [dbo].pInsTranslation 'Character',N'الملصق','Sticker','Edit Lms Literature Detail page',1
EXEC [dbo].pInsTranslation 'Privious',N'السابق','Privious','Edit Lms Literature Detail page',1
EXEC [dbo].pInsTranslation 'send',N'إرسال','send','Edit Lms Literature Detail page',1
EXEC [dbo].pInsTranslation 'Back_to_list',N'القائمة','Back to list','Edit Lms Literature Detail page',1
EXEC [dbo].pInsTranslation 'Back_to_form',N'العودة إلى النموذج','Back to form','Edit Lms Literature Detail page',1
EXEC [dbo].pInsTranslation 'The_book_has_been_successfully_added_to_the_system',N'تم إضافة الكتاب في النظام بنجاح','Book is successfully added in the system','Edit Lms Literature Detail page',1
EXEC [dbo].pInsTranslation 'Book_Updated_Successfully',N'تم تحديث بيانات الكتاب بنجاح','Book detail is successfully updated in the system.','Edit Lms Literature Detail page',1
EXEC [dbo].pInsTranslation 'Add_a_book',N'أضف كتابًا','Add a book','Edit Lms Literature Detail page',1
EXEC [dbo].pInsTranslation 'Are_you_sure_you_want_to_add_the_book?',N'هل أنت متأكد من خطوة إضافة الكتاب؟','Are you sure you want to add the book','Edit Lms Literature Detail page',1
EXEC [dbo].pInsTranslation 'Are_you_sure_you_want_to_ge_rid?',N'.هل أنت متأكد أنك تريد التخلص؟','Are you sure you want to get rid?','Edit Lms Literature Detail page',1
EXEC [dbo].pInsTranslation 'Choose_menu',N'.ختر القائمة','Are you sure you want to get rid?','Edit Lms Literature Detail page',1
EXEC [dbo].pInsTranslation 'Add_new',N'اضف جديد','Add New','Edit Lms Literature Detail page',1 
EXEC [dbo].pInsTranslation 'Existing',N'اختر القائمة','Choose Existing','Edit Lms Literature Detail page',1 
EXEC [dbo].pInsTranslation 'Select_Option',N'اشر على الخيارات','Select Option','Lms Literature Index Divisions page',1


--EditLmsLiteratureIndex page 

EXEC [dbo].pInsTranslation 'Edit_Lms_Literature_Index',N'تعديل الفهرس','Edit Literature Index ','Edit Lms Literature Index page',1 
EXEC [dbo].pInsTranslation 'This_index_number_is_associated_with',N'.يرتبط رقم الفهرس هذا بـ','This Index Number is Associated with','Edit Lms Literature Index page',1
EXEC [dbo].pInsTranslation 'Authors_The_connected_index_number_cannot_be_edited',N'.مؤلفات. رقم الفهرس المتصل لا يمكن تحريره.','Authors. The connected Index Number cannot be Edited.','Edit Lms Literature Index page',1

--EditLmsLiteratureType page  
EXEC [dbo].pInsTranslation 'Arabic_name_required',N'مطلوب اسم عربي','Arabic name required','Edit Lms Literature Type page',1

--Errorlogs page 
EXEC [dbo].pInsTranslation 'Error_logs',N'.سجلات الخطأ','Error logs','Errorlogs page',1
EXEC [dbo].pInsTranslation 'Equal',N'.يساوي','Equal','Errorlogs page',1
EXEC [dbo].pInsTranslation 'is_not_equal',N'.لا يساوي','is_not_equal','Errorlogs page',1
EXEC [dbo].pInsTranslation 'less_than_or_equal_to',N'.أقل من أو يساوي','less than or equal to','Errorlogs page',1
EXEC [dbo].pInsTranslation 'more_than',N'أكثر من','more than','Errorlogs page',1
EXEC [dbo].pInsTranslation 'less_than',N'أقل من','Error logs','Errorlogs page',1
EXEC [dbo].pInsTranslation 'greater_than_or_equal_to',N'.أكبر من أو يساوي','greater than or equal to','Errorlogs page',1
EXEC [dbo].pInsTranslation 'ends_in',N'.ينتهي بـ','ends in','Errorlogs page',1
EXEC [dbo].pInsTranslation 'It_includes',N'.يتضمن','It_includes','Errorlogs page',1
EXEC [dbo].pInsTranslation 'never b',N'.ابدا ب','never b','Errorlogs page',1
EXEC [dbo].pInsTranslation 'Its_not_nothing',N'.هو ليس لاشيء','Its not nothing','Errorlogs page',1
EXEC [dbo].pInsTranslation 'And',N'و','And','Errorlogs page',1
EXEC [dbo].pInsTranslation 'No_logs_to_display.',N'.لا سجلات لعرضها.باطل','No logs to display.','Errorlogs page',1
EXEC [dbo].pInsTranslation 'purifier',N'.منقي','purifier','Errorlogs page',1

--Literature Borrow Approval page 
EXEC [dbo].pInsTranslation 'Agree_to_borrow_books',N'.الموافقة على استعارة المؤلفات','Agree to borrow books','Literature Borrow Approval page',1
EXEC [dbo].pInsTranslation 'To_approve_or_refuse_to_extend_the_loan',N'.لموافقة على تمديد الاقتراض أو الرفض','To approve or refuse to extend the loan','Literature Borrow Approval page',1 

--LiteratureBorrowExtensionApproveReject page 
EXEC [dbo].pInsTranslation 'resolution',N'.قرار','purifier','Literature_Borrow_Approve_Reject page',1
EXEC [dbo].pInsTranslation 'Select_Resolution',N'.حدد القرار','Select Decision','Literature_Borrow_Approve_Reject page',1
EXEC [dbo].pInsTranslation 'Resolution_is_required',N'.مطلوب القرار','Resolution is required','Literature_Borrow_Approve_Reject page',1
EXEC [dbo].pInsTranslation 'Comments_must_be_at_least_two_characters_long',N'يجب أن يتكون التعليق من حرفين على الأقل','Comments must be at least two characters long','Literature_Borrow_Approve_Reject page',1
EXEC [dbo].pInsTranslation 'Comments_must_be_less_than_1000_characters_long',N'يجب ألا يزيد عدد أحرف التعليق عن 1000 حرف','Comments must be less than 1000 characters long','Literature_Borrow_Approve_Reject page',1
EXEC [dbo].pInsTranslation 'Are_you_sure_you_want_to_accept_the_extension_request',N'هل أنت متأكد أنك تريد الموافقة على طلب التمديد','Comments must be less than 1000 characters long','Literature_Borrow_Approve_Reject page',1
EXEC [dbo].pInsTranslation 'The_extension_request_has_been_successfully_approved.',N'.تمت الموافقة على طلب تمديد فترة الاستعارة بنجاح','Borrow period extension request successfully approved','Literature_Borrow_Approve_Reject page',1
EXEC [dbo].pInsTranslation 'Are_you_sure_you_want_to_decline_the_extension_request?.',N'.هل أنت متأكد أنك تريد رفض طلب التمديد؟','Are you sure you want to decline the extension request?.','Literature_Borrow_Approve_Reject page',1
EXEC [dbo].pInsTranslation 'The_loan_extension_request_has_been_successfully_rejected',N'.تم بنجاح رفض طلب تمديد فترة الاستعارة','The loan extension request has been successfully rejected','Literature_Borrow_Approve_Reject page',1
EXEC [dbo].pInsTranslation 'The_loan_approval_request_could_not_be_updated!',N'.تعذر تحديث طلب الموافقة على الاستعارة!','The loan approval request could not be updated!','Literature_Borrow_Approve_Reject page',1


--LmsLiteratureBorrowDetails page  
EXEC [dbo].pInsTranslation 'Enter_keywords',N'.أدخل الكلمات الرئيسية','Enter keywords','Literature Borrow Approval page',1
EXEC [dbo].pInsTranslation 'Reader_name',N'أسماء المستعيرين','User Name','Literature Borrow Approval page',1
EXEC [dbo].pInsTranslation 'ExtendedDueDate ',N'تاريخ انتهاء فترة التمديد','Extended Due Date','AddLmsLiteratureBorrowDetail page',1
EXEC [dbo].pInsTranslation 'Literature_Borrow_Approval_Status',N'Borrow Approval Status','Borrow Approval Status','LmsLiteratureBorrowDetails page',1


--LmsLiteratureClassifications page  
EXEC [dbo].pInsTranslation 'Class_ID',N' معرف التصنيف','Class ID','Lms Literature Classifications page',1 
EXEC [dbo].pInsTranslation 'Create_a_literature_classification',N'.إنشاء تصنيف الأدب','Add literature classification','Lms Literature Classifications page',1

--LmsLiteratureDetailsDialogCard Page 
EXEC [dbo].pInsTranslation 'Literature_name',N'.اسم الأدب','Literature name','Lms Literature Details Dialog Card Page',1
EXEC [dbo].pInsTranslation 'Author_Name',N'اسم المؤلف','Author Name','Lms Literature Details Dialog Card Page',1
EXEC [dbo].pInsTranslation 'number_of_borrowers',N'.عدد المقترضين','number of borrowers','Lms Literature Details Dialog Card Page',1
EXEC [dbo].pInsTranslation 'Number_Of_Borrowed_Books',N'عدد الكتب المستعارة','Number Of Borrowed Books','Lms Literature Details Dialog Card Page',1
EXEC [dbo].pInsTranslation 'Literary_genre_name',N'اسم نوع الكتاب','Number Of Borrowed Books','Lms Literature Details Dialog Card Page',1
EXEC [dbo].pInsTranslation 'Number_of_copies_of_literature',N'.عدد نسخ الأدب','Number of copies of literature','Lms Literature Details Dialog Card Page',1
EXEC [dbo].pInsTranslation 'The_literature_created_by',N'.الأدب الذي أنشأه','The literature created by','Lms Literature Details Dialog Card Page',1
EXEC [dbo].pInsTranslation 'Publications purchase price',N'.سعر شراء المطبوعات ','The literature created by','Lms Literature Details Dialog Card Page',1
EXEC [dbo].pInsTranslation 'Book_purchase_date',N'.تاريخ شراء المؤلفات','Book purchase date','Lms Literature Details Dialog Card Page',1
EXEC [dbo].pInsTranslation 'Literature_is_presentable',N'.الأدب قابل للعرض','Book purchase date','Lms Literature Details Dialog Card Page',1
EXEC [dbo].pInsTranslation 'Literature_classification_name',N'اسم تصنيف الأدب','Literature classification name','Lms Literature Details Dialog Card Page',1
EXEC [dbo].pInsTranslation 'Literature_Catalog_Name',N'اسم فهرس الأدب','Literature Catalogue Name','Lms Literature Details Dialog Card Page',1

--LmsLiteratureIndexDivisions PageEnter keywords

EXEC [dbo].pInsTranslation 'Literature_Index_Division',N'شعبة فهرس الكتاب','Book Index Division','Lms Literature Index Divisions page',1
EXEC [dbo].pInsTranslation 'To_Date',N'حتى تاريخه','To Date','Lms Literature Index Divisions page',1
EXEC [dbo].pInsTranslation 'From_Date',N'من تاريخ','From Date','Lms Literature Index Divisions page',1
EXEC [dbo].pInsTranslation 'Choose_category',N'اختر الفئة','Choose category','Lms Literature Index Divisions page',1
EXEC [dbo].pInsTranslation 'Literature',N'الكتب','Literature','Lms Literature Index Divisions page',1
EXEC [dbo].pInsTranslation 'to_date',N'حتى تاريخه','To date','Lms Literature Index Divisions page',1
EXEC [dbo].pInsTranslation 'Re-Set',N'.إعادة ضبط','Re-Set','Lms Literature Index Divisions page',1
EXEC [dbo].pInsTranslation 'Choose_an_item',N'اختر صنف','Choose_an_item','Lms Literature Index Divisions page',1
EXEC [dbo].pInsTranslation 'Create_Division',N'إضافة رقم القسم','Add Division Number','Lms Literature Index Divisions page',1

--LmsLiteratureTypes Page 
EXEC [dbo].pInsTranslation 'Add_Lms_Literature_Type',N'أضف نوع الكتاب','Add Literature Type','Lms Literature Types page',1
EXEC [dbo].pInsTranslation 'write_the_id',N'اكتب المعرف','write the id','Lms Literature Types page',1
EXEC [dbo].pInsTranslation 'Are_you_sure_you_want_to_delete_this_record?',N'هل أنت متأكد أنك تريد حذف هذا السجل؟','Are you sure you want to delete this record?','Lms Literature Types page',1
EXEC [dbo].pInsTranslation 'Sure_Delete_The_Record?',N'هل أنت متأكد أنك تريد حذف هذا السجل؟','Are you sure you want to delete this record?','Lms Literature Types page',1


--Process Logs

--Wizard Buttons
EXEC [dbo].pInsTranslation 'Assign',N'تعيين','Assign','Wizard page',1
EXEC [dbo].pInsTranslation 'Details',N'تفاصيل','Details','Wizard page',1
EXEC [dbo].pInsTranslation 'Edit',N'تعديل','Edit','Wizard page',1 
EXEC [dbo].pInsTranslation 'Next',N'التالى','Next','Wizard page',1
EXEC [dbo].pInsTranslation 'Previous',N'السابق','Previous','Wizard page',1
EXEC [dbo].pInsTranslation 'Submit',N'إرسال','Submit','Wizard page',1
EXEC [dbo].pInsTranslation 'Label_and_barcode',N'الملصق والباركود','Label and Barcode','Edit Lms Literature Classification page',1
EXEC [dbo].pInsTranslation 'Label_Details',N'تفاصيل الملصق','Label Details','Edit Lms Literature Classification page',1
EXEC [dbo].pInsTranslation 'barcode',N'الباركود','barcode','Edit Lms Literature Classification page',1
EXEC [dbo].pInsTranslation 'Attachments_and_Settings',N'المرفقات والإعدادات','Attachments and Settings','Edit Lms Literature Classification page',1
EXEC [dbo].pInsTranslation 'Barcode_Details',N'تفاصيل الباركود','Barcode Details','Edit Lms Literature Classification page',1
EXEC [dbo].pInsTranslation 'Literature_settings',N'إعدادات الأدب','Literature Settings','Edit Lms Literature Classification page',1
EXEC [dbo].pInsTranslation 'Publish',N'تم نشره','Publish','General',1
EXEC [dbo].pInsTranslation 'Unpublish',N'غير منشور','UnPublish','General',1 
EXEC [dbo].pInsTranslation 'Published',N'تم نشره','Published','General',1 
EXEC [dbo].pInsTranslation 'Add_Child',N'إضافة مبدأ فرعي','Add Child','General',1
 
--Error Statuses
EXEC [dbo].pInsTranslation 'Error',N'خطأ!','Error!','General',1
 
--General
EXEC [dbo].pInsTranslation 'Create_Another',N'إنشاء آخر','Create Another','Lms Literature Types page',1
EXEC [dbo].pInsTranslation 'Loading',N'جاري تحميل','Loading','Lms Literature Types page',1
EXEC [dbo].pInsTranslation 'New',N'الجديد','New','Lms Literature Types page',1
EXEC [dbo].pInsTranslation 'Select',N'حدد','Select','General',1
EXEC [dbo].pInsTranslation 'Add',N'إضافة','Add','General',1
EXEC [dbo].pInsTranslation 'Download',N'يصدّر','Download','General',1 
EXEC [dbo].pInsTranslation 'Advanced_Search',N'البحث المتقدم','Advanced Search','Edit Lms index disivion page',1
EXEC [dbo].pInsTranslation 'Created_Date',N'تاريخ الانشاء','Created Date','General',1
EXEC [dbo].pInsTranslation 'Modified_Date',N'تاريخ التعديل','Modified Date','General',1
EXEC [dbo].pInsTranslation 'Modified_By',N'تاريخ التعديل','Modified By','General',1
EXEC [dbo].pInsTranslation 'Created_By',N'انشاء عن طريق','Created By','General',1
EXEC [dbo].pInsTranslation 'Reset',N'إعادة تعيين','Reset','General',1
EXEC [dbo].pInsTranslation 'No_record_found',N'لا يوجد سجلات','No record found','General',1
EXEC [dbo].pInsTranslation 'Name_En',N'الاسم (انجليزي)','Name (English)','Lms Literature Index page ',1
EXEC [dbo].pInsTranslation 'Name_Ar',N'الاسم (عربي)','Name (Arabic)','Lms Literature Index page ',1 
EXEC [dbo].pInsTranslation 'Author_First_Name_E',N'(100) الاسم الاول ',' First Name (100)','Lms Literature Index page ',1 
EXEC [dbo].pInsTranslation 'Author_Third_Name_E',N'(100) الاسم الأخير',' Last Name (100)','Lms Literature Index page ',1
EXEC [dbo].pInsTranslation 'Author_Address_E',N'(245) العنوان المؤلف ',' Address (245)','Lms Literature Index page ',1 


EXEC [dbo].pInsTranslation 'Author_Third_Name_Ar',N'(100) الاسم الأخير(عربي)',' Last Name (Arabic)(100)','Lms Literature Index page ',1 
EXEC [dbo].pInsTranslation 'Author_First_Name_Ar',N'(100) الاسم الاول (عربي)',' First Name (Arabic)(100)','Lms Literature Index page ',1 
EXEC [dbo].pInsTranslation 'Author_First_Name_En',N'(100)الاسم الاول للمؤلف (انجليزي)',' First Name(English)(100)','Lms Literature Index page ',1 
EXEC [dbo].pInsTranslation 'Author_First_Name_Ar',N'(100)( الاسم الاول (عربي',' First Name (Arabic) (100)','Lms Literature Index page ',1 
EXEC [dbo].pInsTranslation 'Author_First_Name_En',N'(100)الاسم الاول للمؤلف (انجليزي)',' First Name (English) (100)','Lms Literature Index page ',1 
EXEC [dbo].pInsTranslation 'Author_Second_Name_Ar',N'(الاسم الثاني(عربي ',' Second Name (Arabic)','Lms Literature Index page ',1 
EXEC [dbo].pInsTranslation 'Author_Second_Name_En',N'الاسم الثاني (انجليزي)',' Second Name(English)','Lms Literature Index page ',1  
EXEC [dbo].pInsTranslation 'Author_Third_Name_En',N'(100) الاسم الأخير للمؤلف (انجليزي)',' Last Name(English)(100)','Lms Literature Index page ',1 
EXEC [dbo].pInsTranslation 'Author_Address_Ar',N'(245) العنوان المؤلف',' Address (Arabic)(245)','Lms Literature Index page ',1 
EXEC [dbo].pInsTranslation 'Author_Address_En',N'(245) العنوان المؤلف (انجليزي)',' Address(English)(245)','Lms Literature Index page ',1 
EXEC [dbo].pInsTranslation 'Author_Second_Name_En',N'الاسم الثاني (انجليزي)',' Second Name (English)','Lms Literature Index page ',1  
EXEC [dbo].pInsTranslation 'Author_Third_Name_Ar',N'(100) (الاسم الأخير(عربي',' Last Name (Arabic) (100)','Lms Literature Index page ',1 
EXEC [dbo].pInsTranslation 'Author_Third_Name_En',N'(100) الاسم الأخير للمؤلف (انجليزي)',' Last Name (English) (100)','Lms Literature Index page ',1 
EXEC [dbo].pInsTranslation 'Author_Address_Ar',N'(245) العنوان المؤلف',' Address (Arabic) (245)','Lms Literature Index page ',1 
EXEC [dbo].pInsTranslation 'Author_Address_En',N'(245) العنوان المؤلف (انجليزي)',' Address (English) (245)','Lms Literature Index page ',1 
EXEC [dbo].pInsTranslation 'None',N'لا أحد','None','General',1 
EXEC [dbo].pInsTranslation 'ExtendDate',N'تاريخ التمديد','Extend Date','General',1 
EXEC [dbo].pInsTranslation 'BorrowDate',N'تاريتاريخ الاستعارة','Borrow Date','General',1 
EXEC [dbo].pInsTranslation 'OverdueDate',N'تاريخ التأخير','Over due Date','General',1 
EXEC [dbo].pInsTranslation 'BookName',N'اسم الكتاب','Book Name','General',1 
EXEC [dbo].pInsTranslation 'Total_Records',N'إجمالي السجلات:','Total Records:','General',1 
EXEC [dbo].pInsTranslation 'ReturnDate',N'تاريخ العودة','Return Date','General',1 
EXEC [dbo].pInsTranslation 'IssueDate',N'تاريخ الإصدار','Issue Date','General',1 

--FileUpload
EXEC [dbo].pInsTranslation 'File_Name',N'اسم الملف','File Name','General',1
EXEC [dbo].pInsTranslation 'File_Type',N'نوع الملف','File Type','General',1
EXEC [dbo].pInsTranslation 'Doc_Type',N'نوع الوثيقة','Attachment Type','General',1
EXEC [dbo].pInsTranslation 'Date',N'التاريخ','Date','General',1
EXEC [dbo].pInsTranslation 'Accepted_Files',N'الملفات المقبولة:','Accepted Files:','General',1
EXEC [dbo].pInsTranslation 'Sure_Remove_File',N'هل أنت متأكد أنك تريد إزالة هذا الملف؟','Are you sure you want to remove this file?','General',1
EXEC [dbo].pInsTranslation 'Remove_File',N'إزالة الملف','Remove File','General',1
EXEC [dbo].pInsTranslation 'File_Already_Uploaded',N'تم تحميل الملف بالفعل','The file is already uploaded','Add Lds Document Catalogue page',1
EXEC [dbo].pInsTranslation 'Must_Attachment_Type',N'الرجاء تحديد نوع الوثيقة','Please select attachment type','Add Lds Document Catalogue page',1
EXEC [dbo].pInsTranslation 'Select_Attachment_Type',N'حدد نوع المستند','Select Attachment Type','Add Lds Document Catalogue page',1
EXEC [dbo].pInsTranslation 'Attachment_Type',N'نوع الوثيقة','Attachment Type','Add Lds Document Catalogue page',1

--LdsDocument	
EXEC [dbo].pInsTranslation 'Document_Number',N'رقم الوثيقة','Document Number','Documents page',1
EXEC [dbo].pInsTranslation 'Document_Issue_Date',N'تاريخ الاصدار','Document Issue Date','Documents page',1
EXEC [dbo].pInsTranslation 'Document_Applied_Date',N'تاريخ تنفيذ المستند','Document Applied Date','Documents page',1
EXEC [dbo].pInsTranslation 'Document_Issuer',N'مصدر الوثيقة','Document Issuer','Documents page',1
EXEC [dbo].pInsTranslation 'Document_Description',N'وصف المستند','Document Description','Documents page',1
EXEC [dbo].pInsTranslation 'Published_Date',N'تاريخ النشر','Published Date','General',1
EXEC [dbo].pInsTranslation 'Document_View_Page_Title',N'عرض تفاصيل المستند القانوني','Legal Document Details View','Documents page',1
EXEC [dbo].pInsTranslation 'Document_View_Page',N'عرض تفاصيل المستند القانوني','Legal Document Details','Documents page',1
EXEC [dbo].pInsTranslation 'IsAllowedToModify',N'السماح بالتعديل','Allow modification','Documents page',1
EXEC [dbo].pInsTranslation 'Insert',N'إضافة','Insert','Documents page',1


--AddDocument page
EXEC [dbo].pInsTranslation 'Legal_Documents_Title',N'وثيقة قانونية','Legal Documents','Documents page',1
EXEC [dbo].pInsTranslation 'Add_Legal_Document',N'إضافة وثيقة قانونية','Add Legal Document','Add Document page',1
EXEC [dbo].pInsTranslation 'Edit_Legal_Document',N'تعديل الوثيقة القانونية','Edit Legal Document','Edit Document page',1
EXEC [dbo].pInsTranslation 'Select_Subject',N'حدد موضوعات','Select Subject','Add Document page',1
EXEC [dbo].pInsTranslation 'Catalog',N'مجلد','Document Catalogue','Add Document page',1
EXEC [dbo].pInsTranslation 'Select_Catalog',N'حدد فهرس الوثيقة','Select Document Catalogue','Add Document page',1
EXEC [dbo].pInsTranslation 'Document_Type',N'نوع الوثيقة','Document Type','Add Document page',1
EXEC [dbo].pInsTranslation 'Select_Document_Type',N'حدد نوع الوثيقة','Select Document Type','Add Document page',1
EXEC [dbo].pInsTranslation 'Document_Title_Ar',N'العنوان (عربي)','Title (Arabic)','Add Document page',1
EXEC [dbo].pInsTranslation 'Document_Title_En',N'العنوان (الإنجليزية)','Title (English)','Add Document page',1
EXEC [dbo].pInsTranslation 'Government_Entity',N'الجهة الحكومية','Government Entity','Add Document page',1
EXEC [dbo].pInsTranslation 'Select_Government_Entity',N'حدد الجهة الحكومية','Select Government Entity','Add Document page',1
EXEC [dbo].pInsTranslation 'Reference',N'المرجع','Reference','Add Document page',1

EXEC [dbo].pInsTranslation 'Document_Number',N'رقم الوثيقة','Document Number','Add Document page',1
EXEC [dbo].pInsTranslation 'Applied_Date',N'التاريخ المطبق','Applied Date','Add Document page',1
EXEC [dbo].pInsTranslation 'Select_Issuer',N'حدد المصدر','Select Issuer','Add Document page',1
EXEC [dbo].pInsTranslation 'Select_Status',N'حدد الحالة','Select Status','Add Document page',1
EXEC [dbo].pInsTranslation 'Description_Ar',N'وصف','Description Arabic','Add Document page',1
EXEC [dbo].pInsTranslation 'Description_En',N'وصف اللغة الإنجليزية','Description','Add Document page',1
EXEC [dbo].pInsTranslation 'Document_Submitted',N'تم تقديم المستند بنجاح','Document has been submitted successfully','Add Document page',1
EXEC [dbo].pInsTranslation 'Sure_Save_Draft',N'هل أنت متأكد من حفظ المسودة؟	','Are you sure you want to save the draft?','Add Document page',1
EXEC [dbo].pInsTranslation 'Sure_Submit',N'هل أنت متأكد من ارسال الوثيقة؟','Are you sure you want to submit?','Add Document page',1 
EXEC [dbo].pInsTranslation 'Sure_Submit_Literature',N'هل أنت متأكد من ارسال الوثيقة؟','Are you sure you want to submit?','Add Document page',1 
EXEC [dbo].pInsTranslation 'Sure_Delete_Document',N'هل أنت متأكد أنك تريد حذف المستند القانوني المحدد؟','Are sure you want to delete the selected legal document?','Add Document page',1
EXEC [dbo].pInsTranslation 'Document_Deleted_Successfully',N'تم حذف المستند بنجاح','Document has been deleted successfully','Add Document page',1
EXEC [dbo].pInsTranslation 'Metadata_Section',N'قسم المحتوى','Metadata Section','Add Document page',1

--Document Comparison
EXEC [dbo].pInsTranslation 'Content_Section',N'قسم المحتوى','Content Section','Add Document page',1

EXEC [dbo].pInsTranslation 'Attachments_Section',N'قسم المرفقات','Attachments Section','Add Document page',1
EXEC [dbo].pInsTranslation 'Submit_Document',N'إرسال الوثيقة','Submit Document','Add Document page',1
EXEC [dbo].pInsTranslation 'Save_Document_Draft',N'حفظ المسودة','Save Draft','Add Document page',1
EXEC [dbo].pInsTranslation 'Document_Type_Ar',N'النوع (عربي)','Type (Arabic)','Add Document page',1
EXEC [dbo].pInsTranslation 'Document_Type_En',N'النوع (انجليزي)','Type (English)','Add Document page',1 
EXEC [dbo].pInsTranslation 'Fill_Content',N'الرجاء تعبئة محتوى المستند','Please fill document content','Add Document page',1
  
--PublishUnPublishDocument Page	
EXEC [dbo].pInsTranslation 'Publish_and_UnPublish_Document',N'نشر وإلغاء نشر الوثيقة ','Published & Unpublished Documents','Publish Unpublish Document page ',1
EXEC [dbo].pInsTranslation 'Legal_Document_Publish_and_UnPublish',N'نشر/إلغاء نشر الوثائق','Publish/Unpublish Documents','Publish UnPublish Document page ',1
EXEC [dbo].pInsTranslation 'Publish_Confirm',N'هل أنت متأكد أنك تريد النشر','Are you sure you want to publish?','Publish UnPublish Document page ',1
EXEC [dbo].pInsTranslation 'This_file_is_already_published ',N'هل أنت متأكد أنك تريد النشر','This file is already published','Publish UnPublish Document page ',1
 
-- Documents Approval Common
EXEC [dbo].pInsTranslation 'Title',N'العنوان','Title','General',1 
EXEC [dbo].pInsTranslation 'Created_By',N'انشأ من قبل','Created By','General',1 
EXEC [dbo].pInsTranslation 'Issue_Date',N'تاريخ الإصدار','Issue Date','General',1 
EXEC [dbo].pInsTranslation 'Applied_Date',N'تاريخ التطبيق','Applied Date','General',1 

-- Error Statuses
EXEC [dbo].pInsTranslation 'Error',N'خطأ!','Error!','General',1
EXEC [dbo].pInsTranslation 'Success',N'نجاح!','Success!','General',1

-- Documents Approval Dialog
EXEC [dbo].pInsTranslation 'Confirm',N'يتأكد','Confirm!','General',1
EXEC [dbo].pInsTranslation 'OK',N'موافق','OK','General',1
EXEC [dbo].pInsTranslation 'Cancel',N'إلغاء','Cancel','General',1
EXEC [dbo].pInsTranslation 'Confirm_Cancel',N'تأكيد الغاء','Confirm Cancel','General',1  
EXEC [dbo].pInsTranslation 'Created_By',N'انشأ من قبل','Created By','Documents Approval page',1 
EXEC [dbo].pInsTranslation 'Applied_Date',N'تاريخ التطبيق','Applied Date','Documents Approval page',1 
 
-- Documents Approval List Page
EXEC [dbo].pInsTranslation 'Grid_Search',N'بحث','Search','Documents Approval page',1
EXEC [dbo].pInsTranslation 'Export_Button',N'يصدّر','Export','Documents Approval page',1
EXEC [dbo].pInsTranslation 'Filter',N'تصفية','Filter','Documents Approval page',1
EXEC [dbo].pInsTranslation 'And',N'و','And','Documents Approval page',1
EXEC [dbo].pInsTranslation 'Or',N'أو','Or','Documents Approval page',1
EXEC [dbo].pInsTranslation 'Empty',N'فارغة','Empty','Documents Approval page',1
EXEC [dbo].pInsTranslation 'Apply',N'يتقدم','Apply','Documents Approval page',1
EXEC [dbo].pInsTranslation 'Clear',N'واضح','Clear','Documents Approval page',1
EXEC [dbo].pInsTranslation 'Equals',N'يساوي','Equals','Documents Approval page',1
EXEC [dbo].pInsTranslation 'Not_equals',N'لا يساوي','Not equals','Documents Approval page',1
EXEC [dbo].pInsTranslation 'Less_than',N'أقل من','Less than','Documents Approval page',1
EXEC [dbo].pInsTranslation 'Less_than_or_equals',N'أقل من أو يساوي','Less than or equals','Documents Approval page',1
EXEC [dbo].pInsTranslation 'Greater_than',N'أكثر من','Greater than','Documents Approval page',1
EXEC [dbo].pInsTranslation 'Greater_than_or_equals',N'أكبر من أو يساوي','Greater than or equals','Documents Approval page',1
EXEC [dbo].pInsTranslation 'Ends_with',N'ينتهي بـ','Ends with','Documents Approval page',1
EXEC [dbo].pInsTranslation 'Contains',N'يتضمن','Contains','Documents Approval page',1
EXEC [dbo].pInsTranslation 'Does_not_contain',N'لا يحتوي','Does not contain','Documents Approval page',1
EXEC [dbo].pInsTranslation 'Starts_with',N'ابدا ب','Starts with','Documents Approval page',1
EXEC [dbo].pInsTranslation 'Is_not_null',N'هو ليس لاشيء','Is Not Null','Documents Approval page',1
EXEC [dbo].pInsTranslation 'Is_null',N'باطل','Is_null','Documents Approval page',1  
EXEC [dbo].pInsTranslation 'Status',N'الحالة','Status','Documents Approval page',1 

-- Documents Approve/Reject Page
EXEC [dbo].pInsTranslation 'Description_Arabic',N'وصف (عربي)','Description Arabic','Documents Approve/Reject page',1 
EXEC [dbo].pInsTranslation 'Description_English',N'وصف الإنجليزية','Description English','Documents Approve/Reject page',1  
EXEC [dbo].pInsTranslation 'Send',N'يرسل','Send','Documents Approve/Reject page',1 
EXEC [dbo].pInsTranslation 'Decision',N'قرار','Decision','Documents Approve/Reject page',1
EXEC [dbo].pInsTranslation 'Select_Decision',N'حدد القرار ...','Select Decision...','Documents Approve/Reject page',1 
EXEC [dbo].pInsTranslation 'Comment',N'تعليق','Comment','Documents Approve/Reject page',1
EXEC [dbo].pInsTranslation 'Reason',N'سبب','Reason','Documents Approve/Reject page',1
EXEC [dbo].pInsTranslation 'Rejection_Reason',N'سبب الرفض','Rejection Reason','Documents Approve/Reject page',1
  
-- Documents Approve/Reject Page Validations
EXEC [dbo].pInsTranslation 'Required_Decision',N'مطلوب القرار!','Decision is required!','Documents Approve/Reject page',1
EXEC [dbo].pInsTranslation 'Required_Comment_Min_Length',N'يجب أن تتكون التعليقات من حرفين على الأقل!','Comments should be at least 2 characters!','Documents Approve/Reject page',1
EXEC [dbo].pInsTranslation 'Required_Comment_Max_Length',N'يجب ألا تزيد التعليقات عن 1000 حرف!','Comments should be at most 1000 characters!','Documents Approve/Reject page',1
EXEC [dbo].pInsTranslation 'Required_Reason_Min_Length',N'يجب أن يتكون السبب من حرفين على الأقل!','Reason should be at least 2 characters!','Documents Approve/Reject page',1
EXEC [dbo].pInsTranslation 'Required_Reason_Max_Length',N'يجب أن يكون السبب 1000 حرف على الأكثر!','Reason should be at most 1000 characters!','Documents Approve/Reject page',1
 
-- LdsDocumentCatalog page
EXEC [dbo].pInsTranslation 'Page_Title',N'كتالوج الوثائق القانونية',' Documents Catalogue','Lds Document Catalogue page',1
EXEC [dbo].pInsTranslation 'Add_Catalogue',N'اضافة المجلد','Add Catalogue','Lds Document Catalogue page',1
EXEC [dbo].pInsTranslation 'Export_Button',N'يصدّر','Export','Lds Document Catalogue page',1
EXEC [dbo].pInsTranslation 'Export_Button_Excel',N'اكسل','Excel','General',1
EXEC [dbo].pInsTranslation 'Export_Button_CSV',N'ج ق ق','CSV','General',1
EXEC [dbo].pInsTranslation 'Grid_Search',N'بحث','Search','Lds Document Catalogue page',1
EXEC [dbo].pInsTranslation 'Catalogue_Name_Ar',N'اسم المجلد (اللغة العربية)','Catalogue Name (Arabic)','Add Lds Document Catalogue page',1
EXEC [dbo].pInsTranslation 'Catalogue_Name_En',N'اسم المجلد (اللغة الانجليزية)','Catalogue Name (English)','Add Lds Document Catalogue page',1
EXEC [dbo].pInsTranslation 'Catalogue_Name',N'اسم المجلد','Catalogue Name','Add Lds Document Catalogue page',1

EXEC [dbo].pInsTranslation 'CreatedDate',N'تم الانشاء خلال','Created On','Lds Document Catalogue page',1
EXEC [dbo].pInsTranslation 'CreatedBy',N'انشاء عن طريق','Created By','Lds Document Catalogue page',1
EXEC [dbo].pInsTranslation 'Action',N'الاجراءات','Actions','General',1
EXEC [dbo].pInsTranslation 'Validate_Catalogue_Chars_Length',N'يجب أن يتراوح الاسم بين 2 و 250 حرفًا','Name should be between 2 and 250 characters','Lds Document Catalogue page',1
EXEC [dbo].pInsTranslation 'Validate_Catalogue_Chars_Length_Description',N'يجب أن يتراوح الوصف بين 2 و 1000 حرف','Description should be between 2 and 1000 characters','Lds Document Catalogue page',1
EXEC [dbo].pInsTranslation 'Catalogue_Description_Ar',N'وصف الكتالوج (عربي)','Catalogue Description (Arabic)','Add Lds Document Catalogue page',1
EXEC [dbo].pInsTranslation 'Catalogue_Description_En',N'وصف الكتالوج (انجليزي)','Catalogue Description (English)','Add Lds Document Catalogue page',1
EXEC [dbo].pInsTranslation 'Catalogue_Description',N'وصف الكتالوج','Catalogue Description','Add Lds Document Catalogue page',1

--LdsDocumentCatalogue Notification
EXEC [dbo].pInsTranslation 'Catalogue_Created_Successfully',N'تم إنشاء كتالوج المستندات القانونية بنجاح','Legal Documents Catalogue Created Successfully','Lds Document Catalogue Notification',1
EXEC [dbo].pInsTranslation 'Catalogue_Unable_Create',N'تعذر إنشاء كتالوج المستندات القانونية','Unable to create legal document catalogue','Lds Document Catalogue Notification',1
EXEC [dbo].pInsTranslation 'Catalogue_No_Longer_Available',N'هذا الكتالوج لم يعد متاحا','This catalogue is no longer available','Lds Document Catalogue Notification',1
EXEC [dbo].pInsTranslation 'Catalogue_Modified_Successfully',N'تم تعديل كتالوج المستندات القانونية بنجاح','Legal document catalogue is successfully modified','Lds Document Catalogue Notification',1
EXEC [dbo].pInsTranslation 'Catalogue_Unable_Modified',N'غير قادر على تعديل كتالوج المستندات القانونية','Unable to modify legal document catalog','Lds Document Catalogue Notification',1
EXEC [dbo].pInsTranslation 'Catalogue_Delete',N'تمت إزالة الكتالوج وتفاصيله بنجاح من القائمة','The catalogue and its details have been successfully removed from the list','Lds Document Catalogue Notification',1
EXEC [dbo].pInsTranslation 'Catalogue_Unable_Delete',N'تعذر حذف الكتالوج وتفاصيله من القائمة!','The catalogue and its details could not be deleted from the list!','Lds Document Catalogue Notification',1
EXEC [dbo].pInsTranslation 'Catalogue_Delete_Operation_Failed',N'تعذر إجراء عملية الحذف. حاول مرة اخرى.','Unable to perform the delete operation. Please try again.','Lds Document Catalogue Notification',1
EXEC [dbo].pInsTranslation 'Catalogue_Image_already_uploaded',N'تم تحميل الملف بالفعل','The file is already uploaded','Lds Document Catalogue Notification',1
 
--LdsDocumentCatalogue Advanced Search
EXEC [dbo].pInsTranslation 'Catalogue_Advanced_Search_Options',N'قسم البحث','Search Section','Lds Document Catalogue Advanced Search',1
EXEC [dbo].pInsTranslation 'Advanced_Search_Title',N'البحث المتقدم','Advanced Search','Lds Document Catalogue Advanced Search',1
EXEC [dbo].pInsTranslation 'Catalogue_Created_From',N'تم انشاء المجلد من','Catalogue Created From','Lds Document Catalogue Advanced Search',1
EXEC [dbo].pInsTranslation 'Catalogue_Created_To',N'تم انشاء المجلد إلى	','Catalogue Created To','Lds Document Catalogue Advanced Search',1
EXEC [dbo].pInsTranslation 'Catalogue_Creator',N'مُنشأ المجلد','Catalogue Creator','Lds Document Catalogue Advanced Search',1
EXEC [dbo].pInsTranslation 'Catalogue_Enter_Keywords',N'البحث النصي الحر','Free Text Search','Lds Document Catalogue Advanced Search',1
EXEC [dbo].pInsTranslation 'Select_Creator',N'حدد المنشئ','Select Creator','Lds Document Catalogue Advanced Search',1
EXEC [dbo].pInsTranslation 'Catalogue_Reset',N'إعادة ضبط','Reset','Lds Document Catalogue Advanced Search',1
 
--DocumentCatalogueAdd page
EXEC [dbo].pInsTranslation 'Catalogue_Name_Ar',N'اسم المجلد (اللغة العربية)','Catalogue Name (Arabic)','Add Lds Document Catalogue page',1
EXEC [dbo].pInsTranslation 'Catalogue_Name_En',N'اسم المجلد (اللغة الانجليزية)','Catalogue Name (English)','Add Lds Document Catalogue page',1
EXEC [dbo].pInsTranslation 'Catalogue_Description_English',N'المحتوى (اللغة الانجليزية)','Description (English)','Add Lds Document Catalogue page',1
EXEC [dbo].pInsTranslation 'Catalogue_Description_Arabic',N'المحتوى (اللغة العربية)','Description (Arabic)','Add Lds Document Catalogue page',1
EXEC [dbo].pInsTranslation 'Catalogue_Image',N'صورة الفهرس','Catalogue Image','Add Lds Document Catalogue page',1
EXEC [dbo].pInsTranslation 'Validate_Catalogue_Name_En',N'اسم المجلد (اللغة الانجليزية)','Catalogue Name (English) is required','Add Lds Document Catalogue page',1
EXEC [dbo].pInsTranslation 'Validate_Catalogue_Name_Ar',N'مطلوب اسم المجلد (عربي)','Catalogue Name (Arabic) is required','Add Lds Document Catalogue page',1
EXEC [dbo].pInsTranslation 'Validate_Catalogue_Description_English',N'الوصف (اللغة الإنجليزية) مطلوب','Description (English) is required','Add Lds Document Catalogue page',1
EXEC [dbo].pInsTranslation 'Validate_Catalogue_Description_Arabic',N'الوصف (عربي) مطلوب','Description (Arabic) is required','Add Lds Document Catalogue page',1
EXEC [dbo].pInsTranslation 'Validate_Image_Extension',N'يمكنك فقط تحميل ملف JPEG PNG GIF','You can only upload JPEG, PNG & GIF file.','Add Lds Document Catalogue page',1
EXEC [dbo].pInsTranslation 'Catalogue_Create',N'انشاء','Create','Add Lms Literature Index page',1 
EXEC [dbo].pInsTranslation 'Image_Section',N'اختيار الصورة','Image Section','Document Catalog',1

--EditLdsDocumentCatalogue page
EXEC [dbo].pInsTranslation 'Edit_Catalogue_Page',N'تعديل الفهرس','Edit Legal Document Catalogue. ','Edit Lds Document Catalogue page',1 

--DeleteLdsDocumentCatalog page
EXEC [dbo].pInsTranslation 'Delete_Catalogue_Message',N'هل أنت متأكد أنك تريد حذف هذا السجل؟','Are you sure you want to delete this record?','Delete Lds Document Catalogue page',1
EXEC [dbo].pInsTranslation 'Delete_Catalogue_Message_Associated_With_Document',N'كتالوج المستندات هذا مقترن بـ','This Document catalogue is associated with','Delete Lds Document Catalogue page',1
EXEC [dbo].pInsTranslation 'Delete_Catalogue_Message_Associated_With_Document_Second',N'وثائق. هل تريد حذف هذا.','documents. Do you want to delete this.','Delete Lds Document Catalogue page',1 
EXEC [dbo].pInsTranslation 'Editing_Liberary_Borrowing_Details',N'تحرير تفاصيل استعارة الأدب','Editing Liberary Borrowing Details','Edit Lms Literature borrowdetail page',1
EXEC [dbo].pInsTranslation 'Content',N'المحتوى','Content','Add Document page',1
 
--LdsDocumentMasked Grid page
EXEC [dbo].pInsTranslation 'Masked_Page_Title',N'المستندات القانونية المقنعة','Masked legal documents','Lds Document Masked page',1
EXEC [dbo].pInsTranslation 'Masked_Document_Type_Title',N'نوع الوثيقة','Document Type','Lds Document Masked page',1
EXEC [dbo].pInsTranslation 'Masked_Document_Type_Title_Placeholder',N'حدد نوع المستند','Select Document Type','Lds Document Masked page',1
EXEC [dbo].pInsTranslation 'Masked_Content',N'المحتوى المقنع','Masked Content','Lds Document Masked page',1
EXEC [dbo].pInsTranslation 'Masked_CreatedBy',N'انشاء عن طريق','Created By','Lds Document Masked page',1
EXEC [dbo].pInsTranslation 'Masked_CreatedDate',N'تاريخ الانشاء','Created Date','Lds Document Masked page',1
EXEC [dbo].pInsTranslation 'Masked_Create',N'انشاء التظليل	','Create Mask','Lds Document Masked page',1
EXEC [dbo].pInsTranslation 'Masked_IsConfidential',N'هل يعد المبدأ سري','Is Confidential','Lds Document Masked page',1
EXEC [dbo].pInsTranslation 'Masked_Document_Name',N'اسم المستند','Document Name','Lds Document Masked page',1 
EXEC [dbo].pInsTranslation 'Masked_Create',N'انشاء التظليل	','Created Mask','Lds Document Masked page',1

--Legal Document Masked
EXEC [dbo].pInsTranslation 'Create_Document_Mask',N'انشاء التظليل','Create Document Mask','Create Mask',1
EXEC [dbo].pInsTranslation 'Edit_Document_Mask',N'تعديل التظليل','Edit Document Mask','Create Mask',1
EXEC [dbo].pInsTranslation 'Start_Date',N'تاريخ البداية','Start Date','Create Mask',1
EXEC [dbo].pInsTranslation 'End_Date',N'تاريخ الانتهاء','End Date','Create Mask',1
EXEC [dbo].pInsTranslation 'Search_Box',N'صندوق البحث','Search Box','Create Mask',1
EXEC [dbo].pInsTranslation 'Search_Section',N'قسم البحث','Search Section','Create Mask',1
EXEC [dbo].pInsTranslation 'Document_Details',N'تفاصيل الوثيقة','Document Details','Create Mask',1

--- Document Editor
EXEC [dbo].pInsTranslation 'Open',N'ايفتح','Open','Document Editor',1
EXEC [dbo].pInsTranslation 'Separator',N'فاصل','Open','Document Editor',1
EXEC [dbo].pInsTranslation 'Undo',N'الغاء التحميل','Undo','Document Editor',1
EXEC [dbo].pInsTranslation 'Redo',N'إعادة','Redo','Document Editor',1
EXEC [dbo].pInsTranslation 'Image',N'صورة','Image','Document Editor',1
EXEC [dbo].pInsTranslation 'Table',N'الطاولة','Table','Document Editor',1
EXEC [dbo].pInsTranslation 'Hyperlink',N'ارتباط تشعبي','Hyperlink','Document Editor',1
EXEC [dbo].pInsTranslation 'Bookmark',N'المرجعية','Bookmark','Document Editor',1
EXEC [dbo].pInsTranslation 'TableOfContents',N'جدول المحتويات','TableOfContents','Document Editor',1
EXEC [dbo].pInsTranslation 'Header',N'رأس','Bookmark','Document Editor',1
EXEC [dbo].pInsTranslation 'Footer',N'تذييل','Footer','Document Editor',1
EXEC [dbo].pInsTranslation 'PageSetup',N'اعداد الصفحة','PageSetup','Document Editor',1
EXEC [dbo].pInsTranslation 'PageNumber',N'رقم الصفحة','PageNumber','Document Editor',1
EXEC [dbo].pInsTranslation 'Break',N'فترة راحة','Break','Document Editor',1
EXEC [dbo].pInsTranslation 'Find',N'يجد','Find','Document Editor',1
EXEC [dbo].pInsTranslation 'Comments',N'تعليقات','Comments','Document Editor',1
EXEC [dbo].pInsTranslation 'TrackChanges',N'تعقب التغيرات','TrackChanges','Document Editor',1
EXEC [dbo].pInsTranslation 'RestrictEditing',N'تقييد التحرير','RestrictEditing','Document Editor',1
EXEC [dbo].pInsTranslation 'FormFields',N'حقول النموذج','FormFields','Document Editor',1
EXEC [dbo].pInsTranslation 'UpdateFields',N'تحديث الحقول','UpdateFields','Document Editor',1

-- Legal Principle View
EXEC [dbo].pInsTranslation 'Legal_Principles',N'المبدأ القانوني','Legal Principle','Legal Principle View',1
EXEC [dbo].pInsTranslation 'Legal_Principles_Heading',N'المبادئ القانونية','Principles','Legal Principle View',1
EXEC [dbo].pInsTranslation 'Select_Catagory',N'اختر الفئة','Select Category','Legal Principle View',1
EXEC [dbo].pInsTranslation 'Draft',N'مسودة','Draft','Legal Principle View',1
EXEC [dbo].pInsTranslation 'In_Review',N'قيد المراجعة','In Review','Legal Principle View',1
EXEC [dbo].pInsTranslation 'Approve',N'يعتمد','Approve','Legal Principle View',1
EXEC [dbo].pInsTranslation 'Reject',N'رفض','Reject','Legal Principle View',1
EXEC [dbo].pInsTranslation 'Unpublished',N'إلغاء النشر','UnPublished','General',1
EXEC [dbo].pInsTranslation 'Need_To_Modify',N'بحاجة إلى التعديل','Need To Modify','General',1
EXEC [dbo].pInsTranslation 'Send_A_Comment',N'ارسال تعليق','Send A Comment','General',1

-- Legal Principle Add
EXEC [dbo].pInsTranslation 'Principle_Title_Add',N'إضافة مبدأ','Add Principle','Legal Principle Add',1
EXEC [dbo].pInsTranslation 'Principle_Basic_Section',N'القسم الأساسي','Basic Section','Legal Principle Add',1
EXEC [dbo].pInsTranslation 'Principle_Reference_Section',N'قسم المراجع','Reference Section','Legal Principle Add',1
EXEC [dbo].pInsTranslation 'Principle_Category',N'فئة المبدأ','Principle Category','Legal Principle Add',1
EXEC [dbo].pInsTranslation 'Select_Category',N'حدد فئة المبدأ','Select Principle Category','Legal Principle Add',1
EXEC [dbo].pInsTranslation 'Principle_Title',N'العنوان','Title','Legal Principle Add',1
EXEC [dbo].pInsTranslation 'Principle_Number',N'رقم المبدأ','Principle Number','Legal Principle Add',1
EXEC [dbo].pInsTranslation 'Principle_Tag_Placeholder',N'أدخل العلامات المفضلة لديك','Enter your preferred tags','Legal Principle Add',1
EXEC [dbo].pInsTranslation 'Principle_Tags',N'وسم','Tag','Legal Principle Add',1
EXEC [dbo].pInsTranslation 'Legal_Principle_Title',N'المبدأ القانوني','Principle','Legal Principle Add',1 
EXEC [dbo].pInsTranslation 'Principle_IsConfidential',N'هل يعد المبدأ سري','Is Confidential','Legal Principle Add',1
EXEC [dbo].pInsTranslation 'Principle_Add_Reference',N'إضافة المرجع','Add Reference','Legal Principle Add',1
EXEC [dbo].pInsTranslation 'Principle_File_Number',N'حدد رقم الملف','Select File Number','Legal Principle Add',1
EXEC [dbo].pInsTranslation 'Principle_Select_File_Number',N'حدد ملف','Select File','Legal Principle Add',1
EXEC [dbo].pInsTranslation 'Principle_File_Title_Ar',N'عنوان الملف','File Title','Legal Principle Add',1
EXEC [dbo].pInsTranslation 'File_Title',N'عنوان الملف','File Title','Legal Principle Add',1
EXEC [dbo].pInsTranslation 'Principle_File_Type',N'نوع المرجع','Reference Type','Legal Principle Add',1
EXEC [dbo].pInsTranslation 'Principle_Masked_Content_Save',N'حفظ المحتوى','Save Content','Legal Principle Add',1
EXEC [dbo].pInsTranslation 'Principle_Confirmation',N'قسم التأكيد','Confirmation Section','Legal Principle Add',1
EXEC [dbo].pInsTranslation 'Principle_Confirmation_Message',N'يرجى مراجعة وتأكيد التفاصيل الخاصة بك.','Please review and confirm your details.','Legal Principle Add',1
EXEC [dbo].pInsTranslation 'No',N'لا','No','Legal Principle Add',1
EXEC [dbo].pInsTranslation 'Yes',N'نعم','Yes','Legal Principle Add',1
EXEC [dbo].pInsTranslation 'Principle_Version_Number',N'رقم الاصدار','Version Number','Legal Principle Add',1
 
-- Legal Principle Notification
EXEC [dbo].pInsTranslation 'Sure_Delete_Principle',N'هل أنت متأكد من حذف المبدأ القانوني المختار؟','Are sure you want to delete the selected Legal Principle?','Legal Principle Notification',1 
EXEC [dbo].pInsTranslation 'Delete_Principle_Success',N'تم حذف المبدأ القانوني بنجاح','Legal Principle deleted successfully','Legal Principle Notification',1 
EXEC [dbo].pInsTranslation 'Principle_Reference_Delete_Message',N'هل أنت متأكد من حذف المرجع','Are you sure you want to delete the reference?','Legal Principle Notification',1
EXEC [dbo].pInsTranslation 'Principle_Cancel',N'هل أنت متأكد من اغلاق الاستمارة؟','Are you sure you want to close the form?','Legal Principle Notification',1
EXEC [dbo].pInsTranslation 'Sure_you_want_to_unpublish_the_selected_Legal_Principle?',N'هل أنت متأكد من الغاء نشر المبدأ القانوني المختار؟','Are sure you want to unpublish the selected Legal Principle?','Legal Principle Notification',1

-- Legal Principle Version History 
EXEC [dbo].pInsTranslation 'Version_Name',N'اسم الاصدار','Version Name','LegalPrincipleVersionHistory',1
EXEC [dbo].pInsTranslation 'Version_Status',N'حالة الاصدار','Version Status','LegalPrincipleVersionHistory',1
EXEC [dbo].pInsTranslation 'Restore',N'ارجاع','Restore','General',1
EXEC [dbo].pInsTranslation 'Principle_Version_Restore_Successfully',N'تمت استعادة الإصدار الأساسي بنجاح','Principle Version Restored Successfully','General',1
EXEC [dbo].pInsTranslation 'Sure_Delete_Principle_Version',N'هل أنت متأكد من حذف الاصدار؟','Are you sure you want to delete this version?','LegalPrincipleVersionHistory',1
EXEC [dbo].pInsTranslation 'Delete_Principle_Version_Success',N'تم حذف نسخة المبدأ بنجاح','Principle Version Deleted Successfully','LegalPrincipleVersionHistory',1
EXEC [dbo].pInsTranslation 'Legal_Principle_Version_History',N'تاريخ إصدار المبدأ القانوني','Legal Principle Version History','LegalPrincipleVersionHistory',1
EXEC [dbo].pInsTranslation 'Modified_At',N'تم تعديله خلال','Modified At','LegalPrincipleVersionHistory',1
EXEC [dbo].pInsTranslation 'Published_At',N'تم نشره خلال','Published At','LegalPrincipleVersionHistory',1

-- Legal Principle Version History Detail
EXEC [dbo].pInsTranslation 'Version_Name',N'اسم الاصدار','Version Name','LegalPrincipleVersionHistoryDetails',1

-- Legal Document Version History
EXEC [dbo].pInsTranslation 'Version_Number',N'رقم الاصدار','Version Number','LegalDocumentVersionHistoryDetails',1 
 
-- Legal Principle Hierarchy
EXEC [dbo].pInsTranslation 'Legal_Principles_Hierarchy',N'هيكلية المبادئ القانونية','Legal Principles Hierarchy','Legal Principle Hierarchy',1

-- Lms Literatures 







-- Adding New Translations 
EXEC [dbo].pInsTranslation 'IsAllowedToModify',N'السماح بالتعديل','Allow modification','Documents page',1
EXEC [dbo].pInsTranslation 'ModificationAllowMessageDocument',N'سيسمح التحقق من التعديل للمراجع بقراءة/كتابة/تعديل محتوى المستند','Checking Allow Modification will allow the Reviewer to read/write/modify document content.','Documents page',1
EXEC [dbo].pInsTranslation 'ModificationCommentMsg',N':إضافة تعليق للمراجع بالأسفل (اختياري)','Add comments for the Reviewer below (Optional):	','Documents page',1
EXEC [dbo].pInsTranslation 'Content_Collaboration',N'تنسيق النص','Content collaboration	','Documents page',1
EXEC [dbo].pInsTranslation 'Select_Catagory',N'اختر الفئة','Select Category	','Legal Principle View',1
EXEC [dbo].pInsTranslation 'Principle_Title_Add',N'أضف مبدأ قانوني','Add Principle	','Legal Principle Add',1
EXEC [dbo].pInsTranslation 'Principle_Reference_Content_Change_Step',N'يرجى أولاً حفظ محتوى التظليل، من ثم ستتمكن من عمل تغيير','Please first save reference mask content then you will be able to change step.	','Legal Principle Notification',1
EXEC [dbo].pInsTranslation 'Principle_Reference_Required',N'يجب ربط مرجع واحد على الأقل بالمبدأ القانوني','At least 1 reference must be associated with legal principle.','Legal Principle Notification',1
EXEC [dbo].pInsTranslation 'Principle_Saved',N'تم حفظ المبدأ القانوني بنجاح','Principle has been saved successfully	','Legal Principle Notification',1
EXEC [dbo].pInsTranslation 'Principle_Reference_Delete_Error_Message',N'يرجى أولاً حفظ عرض محتوى التظليل، من ثم ستتمكن من حذف المرجع','First save the masking viewer then you will be able to delete reference.','Legal Principle Notification',1
EXEC [dbo].pInsTranslation 'Principle_Reference_Grid_Delete_Success_Message',N'تم حذف المرجع بنجاح','Reference has been deleted successfully','Legal Principle Notification',1
EXEC [dbo].pInsTranslation 'Select_File_Number_Error',N'يرجى أولاً اختيار رقم الملف','First select file number	','Legal Principle Notification',1
EXEC [dbo].pInsTranslation 'Select_Document',N'اختر المستند','Select Document','AddLmsLiteratureBorrowDetail page',1
EXEC [dbo].pInsTranslation 'Sure_you_want_to_publish_the_principle?',N'هل انت متأكد من عمل نشر للمبدأ القانوني؟','Are you sure you want to publish the principle?	','Legal Principle Notification',1
EXEC [dbo].pInsTranslation 'Principle_is_unpublished_successfully',N'تم إلغاء نشر المبدأ القانوني بنجاح','Principle is unpublished successfully	','Legal Principle Notification',1
EXEC [dbo].pInsTranslation 'Principle_is_published_successfully?',N'تم نشر المبدأ القانوني بنجاح','Principle is published successfully	','Legal Principle Notification',1
EXEC [dbo].pInsTranslation 'Principle_is_SendAComment_successfully?',N'تم إرسال التعليق على المبدأ القانوني بنجاح','Principle is Send A Comment successfully	','Legal Principle Notification',1
EXEC [dbo].pInsTranslation 'Sure_Principle_is_SendAComment',N'هل انت متأكد من إرسال تعليق على المبدأ القانوني؟','Are you Sure you want Principle is Send A Comment?	','Legal Principle Notification',1
EXEC [dbo].pInsTranslation 'Sure_Principle_is_inreview',N'هل انت متأكد من إضافة المبدأ القانوني للمراجعة؟','Are you Sure you want Principle is InReview?	','Legal Principle Notification',1
EXEC [dbo].pInsTranslation 'Principle_is_succesfully_inreview',N'تم إضافة المبدأ القانوني للمراجعة بنجاح','Principle is InReview successfully	','Legal Principle Notification',1
EXEC [dbo].pInsTranslation 'Principle_Hierarchy',N'هيكلية المبادئ القانونية ','Legal Principles Hierarchy	','Legal Principles Hierarchy Menu',1 
EXEC [dbo].pInsTranslation 'Document_Reference_Required',N'يجب ربط مرجع واحد على الأقل بالمستند القانوني','At least 1 reference must be associated with legal document.	','Legal document Notification',1
EXEC [dbo].pInsTranslation 'Classification_Delete_Failed',N'لا يمكن حذف التصنيفات المتصلة','Unable to delete connected Classifications!	','Lms Literature Classification Page',1
EXEC [dbo].pInsTranslation 'Save_Reference_Document',N'حفظ مرجع المستند','Save Reference Document	','Add Document page',1
EXEC [dbo].pInsTranslation 'Existing_Reference_Document',N'استخدم المستند المتوفر كمرجع','Use an existing document as a reference	','Add Document page',1
EXEC [dbo].pInsTranslation 'Create_New_Reference_Document',N'تحميل أو إنشاء مستند جديد لإرفاقه كمرجع','Upload or create a new document to attach as a reference','Add Document page',1
EXEC [dbo].pInsTranslation 'Reference_Document_Grid_Title',N'ربط المراجع بالمستند القانوني','Link references to legal document	','Add Document page',1
EXEC [dbo].pInsTranslation 'Reference_Document_Create_Success',N'تم إنشاء مرجع المستند القانوني بنجاح','Reference document create successfully.	','Add Document page',1
EXEC [dbo].pInsTranslation 'FromDate_NotGreater_ToDate',N'من تاريخ يجب ألا يكون أكبر من إلى تاريخ','From Date Should not be Greater Than To Date','Lms Literature page',1
EXEC [dbo].pInsTranslation 'Please_Enter',N'الرجاء إدخال','Please Enter	','General',1 
EXEC [dbo].pInsTranslation 'Document_User',N'مستخدم','User','Add Document page',1
EXEC [dbo].pInsTranslation 'Document_Role',N'الدور','Role','Add Document page',1
EXEC [dbo].pInsTranslation 'Legal_Principle_Version_History_Detail',N'تفاصيل تاريخ إصدارات المبدأ القانوني','Legal Principle Version History Detail	','LegalPrincipleVersionHistoryDetails',1
EXEC [dbo].pInsTranslation 'Legal_Document_Version_History',N'تاريخ إصدارات المستند القانوني','Legal Document Version History	','LegalDocumentVersionHistory',1
EXEC [dbo].pInsTranslation 'Legal_Document_Version_History_Detail',N'تفاصيل تاريخ إصدارات المستند القانوني','Legal Document Version History Detail	','LegalDocumentVersionHistoryDetails',1
EXEC [dbo].pInsTranslation 'Select_Legal_Document_Version_History',N'اختر الإصدار الخاص بالمستند القانوني','Select Legal Document Version History	','SelectLegalDocumentVersionHistory',1
EXEC [dbo].pInsTranslation 'Sure_Publish_Workflow',N'هل انت متأكد من نشر سير العمل؟','Are you sure you want to publish this workflow?	','Workflows page',1
EXEC [dbo].pInsTranslation 'Sure_Active_Workflow',N'لتفعيل سير العمل الجديد سيتم وقف سير العمل الحالي، هل أنت متأكد من تفعيل سير العمل الجديد؟','Making this workflow active will "Suspend" the current active workflow for this trigger. Are you sure you want to activate this workflow?	','Workflows page',1
EXEC [dbo].pInsTranslation 'Sure_Delete_Workflow',N'هل أنت متأكد من حذف سير العمل؟','Are you sure you want to delete this workflow?','Workflows page',1
EXEC [dbo].pInsTranslation 'Literature_Parent_Index_Create_Success',N'تم إنشاء الفهرس الأساسي بنجاح','Parent Index successfully created	','General',1
EXEC [dbo].pInsTranslation 'Literature_Parent_Index_Update_Error',N'حدث خطأ أثناء تعديل الفهرس الأساسي','Some thing went wrong while updating parent index	','General',1
EXEC [dbo].pInsTranslation 'Literature_Parent_Index_Delete_Success',N'تم حذف الفهرس الأساسي بنجاح','Parent Index has been deleted successfully	','General',1
EXEC [dbo].pInsTranslation 'Literature_Parent_Index_Delete_Error',N'حدث خطأ أثناء حذف الفهرس الأساسي','Some thing went wrong while deleting parent index	','General',1
EXEC [dbo].pInsTranslation 'This_division_index_number_is_associated_with',N'رقم القسم والممر مرتبط بـ','This division & aisle number is associated with	','Edit Lms Literature Index page',1
EXEC [dbo].pInsTranslation 'This_division_index_number_is_associated_with_second',N'لا يمكن حذف الرقم','literatures. The connected number cannot be deleted.	','Edit Lms Literature Index page',1
EXEC [dbo].pInsTranslation 'Literature_Parent_Index',N'الفهرس الأساسي للكتاب','Book Parent Index	','Side Bar menu page',1 
EXEC [dbo].pInsTranslation 'Division_aisle_number_is_already_saved',N'تم إدخال رقم القسم والممر مسبقا، يرجى إدخال رقم آخر','Division and Aisle number is already saved, please enter another number.	','General',1
EXEC [dbo].pInsTranslation 'Add_Lms_Literature_Parent_Index',N'إضافة الفهرس','Add Parent Index	','Add Lms Literature Parent Index Page ',1
EXEC [dbo].pInsTranslation 'Parent_Index_Detail',N'تفاصيل فهرس الوالدين','Parent Index Detail','Add child index/edit child index ',1

EXEC [dbo].pInsTranslation 'Parent_Index_Name_En',N'اسم الفهرس الأساسي (انجليزي)','Parent Index Name (English)	','Add Lms Literature LiteratureIndex page ',1
EXEC [dbo].pInsTranslation 'Parent_Index_Name_Ar',N'اسم الفهرس الأساسي (عربي)','Parent Index Name (Arabic)	','Add Lms Literature LiteratureIndex page ',1
EXEC [dbo].pInsTranslation 'Parent_Index_Number',N'رقم الفهرس الأساسي','Parent Index Number	','Lms Literature Index page ',1 
EXEC [dbo].pInsTranslation 'Parent_Index_number_is_already_registered_Please_enter_another_number',N'تم إدخال رقم الفهرس الأساسي مسبقا، يرجى إدخال رقم آخر','Parent index number is already registered. Please enter another.	','General',1
EXEC [dbo].pInsTranslation 'Could_not_create_a_new_index',N'تعذر إنشاء فهرس جديد','Could not create a new index	','General',1
EXEC [dbo].pInsTranslation 'Edit_Lms_Literature_Parent_Index',N'تعديل الفهرس الأساسي','Edit Parent Index	','Add Lms Literature Parent Index page',1 
EXEC [dbo].pInsTranslation 'Parent_index_number_is_associated',N'رقم الفهرس الأساسي مرتبط بـ','This parent index number is associated with	','Lms Literature Parent Index page',1
EXEC [dbo].pInsTranslation 'Parent_index_number_is_associated_message',N'لا يمكن حذف الرقم','literatures. The connected index number cannot be deleted.','Lms Literature Parent Index page',1
EXEC [dbo].pInsTranslation '3_Digit_Number',N'(3 أرقام)','(3 digit number)','Lms Literature Index page',1
EXEC [dbo].pInsTranslation 'Literature_Borrow_Extension_Approve_Reject',N'قسم إعارة الكتب موافقة/رفض','Literature Borrow Extension Approve/Reject','Literature_Borrow_Approve_Reject page',1
EXEC [dbo].pInsTranslation 'Literature_Borrow_Approval_Status',N'حالة الموافقة للإعارة الكتب','Borrow Approval Status','LmsLiteratureBorrowDetails page',1
EXEC [dbo].pInsTranslation 'Add_Type',N'أضف نوع','Add Type','Add Lms Literature Type page',1
EXEC [dbo].pInsTranslation 'Add_Classification',N'أضف تصنيف','Add Classification','Lms Literature Classification page ',1 
EXEC [dbo].pInsTranslation 'Legal_Document_Version_History',N'تاريخ إصدارات المستند القانوني','Legal Document Version History','LegalDocumentVersionHistory',1
EXEC [dbo].pInsTranslation 'Borrow_Approval_Status',N'حالة الموافقة للإعارة الكتب','Status','AddLmsLiteratureBorrowDetail page',1
EXEC [dbo].pInsTranslation 'Book_Status',N'حالة الكتاب','Book Status','Lms Literatures',1
EXEC [dbo].pInsTranslation 'User_Management',N'إدارة المستخدمين','User Management','UMS',1
EXEC [dbo].pInsTranslation 'Users',N'المستخدمين','Users','UMS',1
EXEC [dbo].pInsTranslation 'Roles',N'الأدوار','Roles','UMS',1
EXEC [dbo].pInsTranslation 'Groups',N'المجموعات','Groups','UMS',1
EXEC [dbo].pInsTranslation 'Sure_Save_Role',N'هل أنت متأكد من حفظ الدور؟','Are you sure you want to save this role?','UMS Roles',1
EXEC [dbo].pInsTranslation 'Sure_Save_Changes',N'هل أنت متأكد من حفظ التغييرات؟','Are you sure you want to save changes?','UMS Roles',1
EXEC [dbo].pInsTranslation 'Create_Role',N'إنشاء دور','Create Role','UMS Roles',1
EXEC [dbo].pInsTranslation 'Roles',N'الأدوار','Roles','UMS Roles',1
EXEC [dbo].pInsTranslation 'Update_Role',N'تعديل الدور','Update Role','UMS Roles',1
EXEC [dbo].pInsTranslation 'Save_Changes',N'حفظ التغييرات','Save Changes','UMS Roles',1
EXEC [dbo].pInsTranslation 'Role_Name_Required',N'يجب إدخال اسم الدور، يرجى إدخال اسم الدور','Role Name is required. Kindly enter the Role Name.','UMS Roles',1
EXEC [dbo].pInsTranslation 'Role_Details',N'تفاصيل الدور','Role Details','UMS Roles',1
EXEC [dbo].pInsTranslation 'Role_Id',N'رقم الدور','Role Id','UMS Roles',1
EXEC [dbo].pInsTranslation 'Role_Name',N'اسم الدور','Role Name','UMS Roles',1
EXEC [dbo].pInsTranslation 'Created_At',N'تم إنشائه عن طريق','Created At','UMS Roles',1
EXEC [dbo].pInsTranslation 'Modified_At',N'تم تعديله عن طريق','Modified At','UMS Roles',1
EXEC [dbo].pInsTranslation 'Role_Description',N'وصف الدور','Role Description','UMS Roles',1
EXEC [dbo].pInsTranslation 'Clone_Permissions',N'نسخ الصلاحيات','Clone Permissions','UMS Roles',1
EXEC [dbo].pInsTranslation 'Select_Role_To_Clone',N'اختر الدور لنسخ كل الصلاحيات','Select a role to clone all the permissions assigned to it.','UMS Roles',1
EXEC [dbo].pInsTranslation 'Role',N'الدور','Role','UMS Roles',1
EXEC [dbo].pInsTranslation 'Clone',N'نسخ','Clone','UMS Roles',1
EXEC [dbo].pInsTranslation 'Role_Description_En',N'وصف الدور (انجليزي)','Role Description (English)','UMS Roles',1
EXEC [dbo].pInsTranslation 'Role_Description_Ar',N'وصف الدور (عربي)','Role Description (Arabic)','UMS Roles',1
EXEC [dbo].pInsTranslation 'Unauthorized',N'غير مصرح! هناك خطأ بالتصريح الخاص بك','Unauthorized! Your authorization failed.','UMS Roles',1
EXEC [dbo].pInsTranslation 'Role_Description_3_dot',N'...','...','UMS Roles',1
EXEC [dbo].pInsTranslation 'User_Groups',N'مجموعات المستخدمين','User Groups','Ums Group page',1
EXEC [dbo].pInsTranslation 'Update_Group',N'تعديل المجموعة','Update Group','Ums Group page',1
EXEC [dbo].pInsTranslation 'Groups',N'المجموعات','Groups','Ums Group page',1
EXEC [dbo].pInsTranslation 'Create_Group',N'إنشاء مجموعات','Create Groups','Ums Group page',1
EXEC [dbo].pInsTranslation 'Title',N'العنوان','Title','UMS Roles',1
EXEC [dbo].pInsTranslation 'Module',N'الوحدة','Module','UMS Roles',1
EXEC [dbo].pInsTranslation 'SubModule',N'الوحدة الفرعية','SubModule','UMS Roles',1
EXEC [dbo].pInsTranslation 'Value',N'	القيمة','Value','UMS Roles',1
EXEC [dbo].pInsTranslation 'FirstName_Ar',N'(الاسم الأول (عربي)','FirstName (Arabic)','Ums User page',1
EXEC [dbo].pInsTranslation 'FirstName_En',N'	(الاسم الأول (انجليزي)','FirstName (English)','User Groups page',1
EXEC [dbo].pInsTranslation 'SecondName_Ar',N'	(الاسم الثاني (عربي)','SecondName (Arabic)','Ums User page',1
EXEC [dbo].pInsTranslation 'SecondName_En',N'	(الاسم الثاني (انجليزي)','SecondName (English)','Ums User page',1
EXEC [dbo].pInsTranslation 'Mobile_Number',N'	رقم الهاتف المقال','Mobile Number','Ums User page',1
EXEC [dbo].pInsTranslation 'Group_Name_En',N'  (اسم المجموعة (انجليزي','Group Name (English)','Ums Group page',1
EXEC [dbo].pInsTranslation 'Group_Name_Ar',N'   ( اسم المجموعة (عربي','Group Name (Arabic)','Ums Group page',1
EXEC [dbo].pInsTranslation 'Group_Description_En',N'	وصف المجموعة (انجليزي)','Group Description (English)','Ums Group page',1
EXEC [dbo].pInsTranslation 'Group_Description_Ar',N'	وصف المجموعة (عربي)','Group Description (Arabic)','Ums Group page',1
EXEC [dbo].pInsTranslation 'Delete_Selected_User_Group',N'	حذف المجموعات المختارة','Delete Selected Groups','Ums Group page',1
EXEC [dbo].pInsTranslation 'Select_User_Group_For_Deletion_Message',N'يرجى اختيار المجموعة المراد حذفها','Please first select group for deletion.','Ums Group page',1
EXEC [dbo].pInsTranslation 'Select_Group_Deletion_Success_Message',N'تم حذف المجموعة المختارة بنجاح','Selected group delete successfully.','Ums Group page',1
EXEC [dbo].pInsTranslation 'fill_Required_Grids',N'	يرجى تعبئة كلا الجدولين','Please Fill Both Grids','Ums Group page',1
EXEC [dbo].pInsTranslation 'Users',N'	المستخدمين','Users','Ums User page',1
EXEC [dbo].pInsTranslation 'Create_User',N'	إنشاء مستخدم','Create User','Ums User page',1
EXEC [dbo].pInsTranslation 'User_Id',N'	رقم المستخدم','User Id','Ums User page',1
EXEC [dbo].pInsTranslation 'User_Mobile_Number',N'	رقم الهاتف المقال','Mobile Number','Ums User page',1
EXEC [dbo].pInsTranslation 'User_Name_Arabic',N'	الاسم (عربي)','Name (Arabic)','Ums User page',1
EXEC [dbo].pInsTranslation 'User_Name_English',N'	الاسم (انجليزي)','Name (English)','Ums User page',1
EXEC [dbo].pInsTranslation 'User_Department_Arabic',N'	اسم القسم (عربي)','Department Name (Arabic)','Ums User page',1
EXEC [dbo].pInsTranslation 'User_Department_English',N'اسم القسم (انجليزي)','Department Name (English)','Ums User page',1
EXEC [dbo].pInsTranslation 'User_Type_Arabic',N'	اسم النوع (عربي)','Type Name (Arabic)','Ums User page',1
EXEC [dbo].pInsTranslation 'User_Type_English',N'	اسم النوع (انجليزي)','Type Name (English)','Ums User page',1 
EXEC [dbo].pInsTranslation 'Create_Group',N'	إنشاء مجموعة','Create Group','User Groups page',1
EXEC [dbo].pInsTranslation 'Update_User',N'	تعديل المستخدم','Update User','Save User page',1
EXEC [dbo].pInsTranslation 'LastName_En',N'الاسم الأخير (انجليزي)','Last Name (English)','Save User page',1
EXEC [dbo].pInsTranslation 'LastName_Ar',N'	الاسم الأخير (عربي)','Last Name (Arabic)','Save User page',1
EXEC [dbo].pInsTranslation 'Date_Of_Birth',N'	تاريخ الميلاد','Date Of Birth','Save User page',1
EXEC [dbo].pInsTranslation 'Address',N'	العنوان','Address','Save User page',1 
EXEC [dbo].pInsTranslation 'Date_Of_Joining',N'تاريخ التعيين','Date Of Joining','Save User page',1
EXEC [dbo].pInsTranslation 'User_Group',N'	مجموعة المستخدمين','User Group','Save User page',1
EXEC [dbo].pInsTranslation 'Alternate_Mobile_Number',N'رقم هاتف نقال آخر ','Alternate Mobile Number','Save User page',1
EXEC [dbo].pInsTranslation 'Nationality',N'	الجنسية','Nationality','Save User page',1
EXEC [dbo].pInsTranslation 'Select_Nationality',N'	اختر الجنسية','Select Nationality','Save User page',1
EXEC [dbo].pInsTranslation 'Gender',N'	الجنس','Gender','Save User page',1
EXEC [dbo].pInsTranslation 'Select_Gender',N'	اختر الجنس','Select Gender','Save User page',1
EXEC [dbo].pInsTranslation 'User_Grade',N'	الدرجة الوظيفية','User Grade','Save User page',1
EXEC [dbo].pInsTranslation 'Select_User_Grade',N'	اختر الدرجة الوظيفية','Select User Grade','Save User page',1
EXEC [dbo].pInsTranslation 'User_Type',N'	نوع المستخدم','User Type','Save User page',1

EXEC [dbo].pInsTranslation 'Select_User_Type',N'اختر نوع المستخدم','Select User Type','Save User page',1
EXEC [dbo].pInsTranslation 'Designation',N'	المسمى الوظيفي','Designation','Save User page',1
EXEC [dbo].pInsTranslation 'Select_User_Designation',N'	اختر المسمى الوظيفي','Select User Designation','Save User page',1
EXEC [dbo].pInsTranslation 'Manager',N'	 اسم المدير','Manager','Save User page',1
EXEC [dbo].pInsTranslation 'Select_User_Manager',N'	اختر اسم المدير','Select User Manager','Save User page',1
EXEC [dbo].pInsTranslation 'Is_Active',N'	مفعل','Is Active','Save User page',1
EXEC [dbo].pInsTranslation 'Allow_Access',N'السماح بالدخول على النظام','Allow Access','Save User page',1
EXEC [dbo].pInsTranslation 'Is_Locked',N'مقفل','Is Locked','Save User page',1
EXEC [dbo].pInsTranslation 'Absent_From',N'غير متواجد من تاريخ','Absent From','Save User page',1
EXEC [dbo].pInsTranslation 'Absent_To',N'غير متواجد إلى تاريخ','Absent To','Save User page',1
EXEC [dbo].pInsTranslation 'Replacement',N'	الموظف البديل','Replacement','Save User page',1
EXEC [dbo].pInsTranslation 'Select_User_Replacement',N'اختر الموظف البديل','Select User Replacement','Save User page',1
EXEC [dbo].pInsTranslation 'Upload_Profile_Pic',N'تحميل صورة الملف الشخصي','Upload Profile Pic','Save User page',1
EXEC [dbo].pInsTranslation 'Confirm_Password',N'تأكيد كلمة المرور','Confirm Password','Save User page',1
EXEC [dbo].pInsTranslation 'User_Role',N'	دور المستخدم','User Role','Save User page',1
EXEC [dbo].pInsTranslation 'Select_User_Role',N'	اختر دور المستخدم','Select User Role','Save User page',1
EXEC [dbo].pInsTranslation 'Department',N'	القسم','Department','Save User page',1
EXEC [dbo].pInsTranslation 'Select_User_Department',N'اختر القسم','Select User Department','Save User page',1
EXEC [dbo].pInsTranslation 'Permissions',N'	الصلاحيات','Permissions','Save User page',1
EXEC [dbo].pInsTranslation 'Title',N'	العنوان','Title','Save User page',1
EXEC [dbo].pInsTranslation 'Selected',N'	تم اختياره','Selected','Save User page',1
EXEC [dbo].pInsTranslation 'Create',N'	إنشاء','Create','Save User page',1



















--Lms Literature Details


	EXEC [dbo].pInsTranslation 'Literature_Name',N'اسم الأدب ','Literature Name ','LMS literature Detail page ',1
--EXEC [dbo].pInsTranslation 'Literature_Name_Ar',N'Literature Name (Arbaic)','Literature Name (Arbaic)','LMS literature Detail page ',1
EXEC [dbo].pInsTranslation 'Approved',N'اعتماد','Approved','Legal principle page ',1	
EXEC [dbo].pInsTranslation 'Rejected',N'رفض','Rejected','Legal principle page ',1	
EXEC [dbo].pInsTranslation 'Published',N'تم نشره','Published','Legal principle page ',1	
EXEC [dbo].pInsTranslation 'In Review',N'قيد المراجعة','InReview','Legal principle page ',1	

EXEC [dbo].pInsTranslation 'Sure_Delete_Document_Version',N'هل أنت متأكد من حذف الاصدار؟','Are you sure you want to delete this version?','LegalPrincipleVersionHistory',1






--added new missing translations(8-9-22){
EXEC [dbo].pInsTranslation 'View_Legal_Principle_Versions',N'عرض اصدارات المبادئ القانونية','View Legal Principle Versions','LegalPrincipleVersionHistoryDetails',1
EXEC [dbo].pInsTranslation 'View_Legal_Principle_Version_Detail',N'عرض تفاصيل اصدار المبدأ القانوني','View Legal Principle Version Detail','LegalPrincipleVersionHistoryDetails',1
EXEC [dbo].pInsTranslation 'Add_Tag',N'إضافة وسم','Add Tag','Literature',1
EXEC [dbo].pInsTranslation 'TagNo',N'رقم الوسم','Tag No','Literature',1
EXEC [dbo].pInsTranslation 'Value',N'القيمة','Value','Literature',1

EXEC [dbo].pInsTranslation 'Tag_Already_Added',N'تمت اضافة الوسم المحدد مسبقا','The selected Tag has been already added.','Literature',1
EXEC [dbo].pInsTranslation 'Publisher_Details',N'(260) بيانات المؤلف','Publisher Details (260)','Literature',1
EXEC [dbo].pInsTranslation 'Active',N'نشط','Active','Literature',1
EXEC [dbo].pInsTranslation 'Increment_Copies',N'تزايد النسخ','Increment Copies','Literature',1
EXEC [dbo].pInsTranslation 'Atleast_One_Book_Copy',N'نسخة كتاب واحدة على الأقل','There should be at least one copy of the book.','Literature',1

EXEC [dbo].pInsTranslation 'Role_Deleted',N'تم حذف الدور الوظيفي بنجاح','Role has been deleted successfully','UMS Roles',1
EXEC [dbo].pInsTranslation 'User_Detail_View_Title',N' عرض بيانات المستخدم','User Detail View','User Groups page',1
 
EXEC [dbo].pInsTranslation 'UserName_Already_Exist',N'اسم المستخدم موجود مسبقا','UserName Already Exist','Save User page',1
EXEC [dbo].pInsTranslation 'Sure_Save_User',N'تأكد من حفظ المستخدم','Sure Save User','Save User page',1
EXEC [dbo].pInsTranslation 'User_Add_Success',N'نجاح إضافة المستخدم','User Add Success','Save User page',1
EXEC [dbo].pInsTranslation 'Sure_Cancel_Save_User',N'تأكد من إلغاء حفظ المستخدم','Sure Cancel Save User','Save User page',1 
EXEC [dbo].pInsTranslation 'Module',N'الوحدة','Module','Save User page',1
EXEC [dbo].pInsTranslation 'Submodule',N'الوحدة الفرعية','Submodule','Save User page',1
EXEC [dbo].pInsTranslation 'Token_Expired',N'انتهت صلاحية الرمز','Token Expired','Save User page',1
EXEC [dbo].pInsTranslation 'Role_Name_Exists',N'اسم الدور الوظيفي موجود مسبقا','Role Name entered already exists','Save User page',1


EXEC [dbo].pInsTranslation 'Transfer_User',N'نقل المستخدم','Transfer User','Ums Transfer User page',1
EXEC [dbo].pInsTranslation 'Transfer_UserName',N'اسم المستخدم / الرقم','UserName/Id','Ums Transfer User page',1
EXEC [dbo].pInsTranslation 'Select_UserName',N'اختر اسم المستخدم','Select UserName','Ums Transfer User page',1
EXEC [dbo].pInsTranslation 'Transfer_CurrentDept',N'القسم الحالي','Current Department','Ums Transfer User page',1
EXEC [dbo].pInsTranslation 'Transfer_NextDept',N'تحويل إلى','Transfer To','Ums Transfer User page',1
EXEC [dbo].pInsTranslation 'Select_Department',N'اختر القسم','Select Department','Ums Transfer User page',1
EXEC [dbo].pInsTranslation 'Transfer_Start_Date',N'تاريخ البداية ','Start Date','Ums Transfer User page',1
EXEC [dbo].pInsTranslation 'Transfer_End_Date',N'تاريخ الانتهاء','End Date','Ums Transfer User page',1
EXEC [dbo].pInsTranslation 'Transfer_Permission_Change_Message',N'هل تريد تغيير صلاحية المستخدم الحالي؟','Do you want to change user current permission?','Ums Transfer User page',1
EXEC [dbo].pInsTranslation 'Transfer_History',N'تاريخ التحويل','Transfer History','Ums Transfer User page',1
EXEC [dbo].pInsTranslation 'Transfer_Date_Check',N'يجب ألا يكون تاريخ الانتهاء أكبر من تاريخ البداية','End date should not be greater than start date','Ums Transfer User page',1
EXEC [dbo].pInsTranslation 'Save_Transfer_Success',N'تم نقل المستخدم بنجاح','User transfer successfully','Ums Transfer User page',1
 
EXEC [dbo].pInsTranslation 'User_Validation_length',N'يجب أن يتكون اسم المستخدم من 2 إلى 50 حرفًا','User Name Should Be Between 2 To 50 Characters ','Save User page',1


EXEC [dbo].pInsTranslation 'Phone_is_required',N'رقم الهاتف مطلوب','Phone Number Is Required','Save User page',1

EXEC [dbo].pInsTranslation 'Invalid_Phone_Number',N'رقم الهاتف غير صالح','Not a valid Phone Number','Save User page',1


EXEC [dbo].pInsTranslation 'Email_is_required',N'البريد الالكتروني مطلوب','Email Address Is Required ','Save User page',1

EXEC [dbo].pInsTranslation 'Invalid_Email',N'البريد الالكتروني غير صالح','Not a valid Email','Save User page',1

EXEC [dbo].pInsTranslation 'Email_InValid_length',N'البريد الالكتروني طويل جدا','That Email is too long','Save User page',1

EXEC [dbo].pInsTranslation 'Address_is_required',N'العنوان مطلوب','Address Is Required ','Save User page',1

EXEC [dbo].pInsTranslation 'Address_Invalid_Length',N'العنوان طويل جدا','Address Is Too Long','Save User page',1

EXEC [dbo].pInsTranslation 'DOB_is_required',N'تاريخ الميلاد مطلوب','Date Of Birth Is Required ','Save User page',1

EXEC [dbo].pInsTranslation 'DOJ_is_required',N'تاريخ التعيين مطلوب','Date Of Joining Is Required ','Save User page',1


EXEC [dbo].pInsTranslation 'Nationality_is_required',N'الجنسية مطلوبة','Nationality Is Required','Save User page',1

EXEC [dbo].pInsTranslation 'Gender_is_required',N'الجنس مطلوب','Gender Is Required','Save User page',1

EXEC [dbo].pInsTranslation 'Password_is_required',N'كلمة المرور مطلوبة','Password Is Required','Save User page',1

EXEC [dbo].pInsTranslation 'Confirm_Password_Required',N'مطلوب تأكيد كلمة المرور','Confirm Password Is Required','Save User page',1

EXEC [dbo].pInsTranslation 'Invalid_Password_Length',N'يجب أن يتراوح طول كلمة المرور بين 8 إلى 100 حرف','Password should be between 8 to 100 characters long','Save User page',1

EXEC [dbo].pInsTranslation 'Unmatched_Password',N'كلمات المرور لا تتطابق','Passwords Dont Match','Save User page',1

EXEC [dbo].pInsTranslation 'policy_for_User',N'يجب أن تحتوي كلمة المرور على حرف كبير ورقم وحرف خاص واحد على الأقل','Password Must Contain atleast 1 Capital Letter, Number & Special Character','Save User page',1

EXEC [dbo].pInsTranslation 'Department_is_required',N'يرجى تحديد القسم الخاص بك','Please Select Your Department','Save User page',1
EXEC [dbo].pInsTranslation 'Save_Transfer_Success',N'تم نقل المستخدم بنجاح','User transfer successfully','Ums Transfer User page',1
 
EXEC [dbo].pInsTranslation 'System_Config_Title',N'اعدادات النظام','System Configuration','System Configuration page',1
EXEC [dbo].pInsTranslation 'System_Config_Add_Title',N'اضافة اعدادت النظام','Add System Configuration','System Configuration add page',1
EXEC [dbo].pInsTranslation 'Select_Group',N'اختر المجموعة','Select Group','System Configuration add page',1
EXEC [dbo].pInsTranslation 'System_Required_Field',N'القيمة مطلوبة','Value is required','System Configuration add page',1
EXEC [dbo].pInsTranslation 'Password_Period',N'المدة الزمنية لكلمة المرور','Password Period','System Configuration add page',1
EXEC [dbo].pInsTranslation 'Select_Option',N'حدد الخيار','Select Option','System Configuration add page',1
EXEC [dbo].pInsTranslation 'Password_Policy',N'سياسة كلمة المرور','Password Policy','System Configuration add page',1
EXEC [dbo].pInsTranslation 'Session_Period',N'المدة الزمنية لانتهاء الجلسة','Session Timeout Period','System Configuration add page',1
EXEC [dbo].pInsTranslation 'Wrong_Password_Attempts',N'عدد محاولات كلمة المرور الخاطئة','Number of Wrong Password Attempts','System Configuration add page',1
EXEC [dbo].pInsTranslation 'Invalid_Login_Attempts',N'عدد محاولات تسجيل الدخول غير الصالحة','Number of Invalid Login Attempts','System Configuration add page',1
EXEC [dbo].pInsTranslation 'Confirmation_Message',N'هل أنت متأكد من حفظ اعدادات النظام؟','Are you sure you want to save the system configuration?','System Configuration add page',1
EXEC [dbo].pInsTranslation 'Success_Messsage',N'تم حفظ اعدادات النظام بنجاح','System configuration saved successfully','System Configuration add page',1
EXEC [dbo].pInsTranslation 'In_Minutes',N'في دقيقة','In Minute','System Configuration add page',1
EXEC [dbo].pInsTranslation 'Password_Policy_Section',N'قسم سياسة كلمة المرور','Password Policy Section','System Configuration add page',1
EXEC [dbo].pInsTranslation 'Select_Policy',N'اختر سياسة كلمة المرور','Select Password Policy','System Configuration add page',1
EXEC [dbo].pInsTranslation 'Enter_Value',N'قم بادخال القيمة','Enter Value','System Configuration add page',1
EXEC [dbo].pInsTranslation 'Select_Value',N'حدد القيمة','Choose Value','System Configuration add page',1
EXEC [dbo].pInsTranslation 'Rule_Name_En',N'اسم القانون باللغة الإنجليزية','Rule Name English','System Configuration add page',1
EXEC [dbo].pInsTranslation 'Rule_Name_Ar',N'اسم القانون باللغة العربية','Rule Name Arabic','System Configuration add page',1
EXEC [dbo].pInsTranslation 'Rule_Value',N'قيمة القانون','Rule Value','System Configuration add page',1
EXEC [dbo].pInsTranslation 'System_Config_Edit_Title',N'تحرير اعدادات النظام','Edit System Configuration','System Configuration edit page',1
EXEC [dbo].pInsTranslation 'Update_Success_Messsage',N'تم تحديث اعدادات النظام بنجاح','System configuration updated successfully','System Configuration edit page',1
EXEC [dbo].pInsTranslation 'Required_Field',N'القيمة المطلوبة','Value required','System Configuration edit page',1
 
EXEC [dbo].pInsTranslation 'Case_Respond_Button',N'رد','Respond','Case Respond form',1
EXEC [dbo].pInsTranslation 'Select_Response_Type',N'حدد نوع الرد','Select Response Type','Case Respond form',1
EXEC [dbo].pInsTranslation 'Response_Others_Conditional',N'ادخل القيمة','Enter Value','Case Respond form',1
EXEC [dbo].pInsTranslation 'Select_Response_Reason',N'اختار السبب','Select Reason','Case Respond form',1
EXEC [dbo].pInsTranslation 'Response_Comments',N'ادخل التعليقات','Enter Comments','Case Respond form',1
EXEC [dbo].pInsTranslation 'Response_Justifications',N'أدخل المبررات','Enter Justifications','Case Respond form',1
EXEC [dbo].pInsTranslation 'Response_DueDate',N'ادخل تاريخ الانتهاء المحدد','Select Due Date','Case Respond form',1
EXEC [dbo].pInsTranslation 'Select_Reminder',N'اختر التنبيه','Select Reminder','Case Respond form',1
EXEC [dbo].pInsTranslation 'Response_Is_Urgent',N'هل هو مستعجل','Is Urgent','Case Respond form',1
EXEC [dbo].pInsTranslation 'Send',N'ارسال','Send','Case Respond form',1
EXEC [dbo].pInsTranslation 'Response_Confirmation_Message',N'هل أنت متأكد من إرسال هذا الرد؟','Are you sure to send this response?','Case Respond form',1
EXEC [dbo].pInsTranslation 'Yes',N'نعم','Yes','Case Respond form',1
EXEC [dbo].pInsTranslation 'No',N'لا','No','Case Respond form',1
EXEC [dbo].pInsTranslation 'Case_Respond_Button',N'رد','Respond','Case Respond form',1
EXEC [dbo].pInsTranslation 'Version_History',N'تاريخ الاصدار','Version History','Document',1
EXEC [dbo].pInsTranslation 'Modified_Date',N'تاريخ التعديل','Modified Date ','Legal Principle',1
EXEC [dbo].pInsTranslation 'Submitted_Date',N'تاريخ الاضافة','Submitted Date ','Legal Principle',1
 
EXEC [dbo].pInsTranslation 'Password_Alteast_Number',N'يجب ان تحتوي كلمة المرور على الاقل رقما واحدا','Password must contain at least one number','Password Policy Validations',1
EXEC [dbo].pInsTranslation 'Password_Minimum_Length',N'يجب أن تتوافق كلمة المرور مع الحد الأدنى في الطول','Password must meet minimum characters in length','Password Policy Validations',1
EXEC [dbo].pInsTranslation 'Password_Lower_Upper',N'يجب أن تحتوي كلمة المرور على أحرف كبيرة وصغيرة','Password must contain both uppercase and lowercase characters','Password Policy Validations',1
EXEC [dbo].pInsTranslation 'Passwor_Special_Character',N'يجب أن تحتوي كلمة المرور على حرف خاص واحد على الأقل','Password must contain at least one special character','Password Policy Validations',1

EXEC [dbo].pInsTranslation 'Publisher_Name',N'اسم المؤلف','Publisher Name','Legal Principle',1
EXEC [dbo].pInsTranslation 'Published_Date',N'تاريخ النشر','Published Date','Legal Principle',1
 
EXEC [dbo].pInsTranslation 'Add_Hierarchy',N'اضافة','Add','Principle Hierarchy page',1
 
 
EXEC [dbo].pInsTranslation 'Index_Division_Show',N'أقسام','Division','Lms Literature Index page',1
EXEC [dbo].pInsTranslation 'Index_Number_Range_Dot_Check',N'يسمح فقط برقم ونقطة واحدة','Only number one dot allow','Lms Literature Index page',1


EXEC [dbo].pInsTranslation 'Index_Update',N'تحديث','Update','Edit Lms Literature Index page',1

EXEC [dbo].pInsTranslation 'Literature_Borrow_Detail',N'الكتب المستعارة','Borrowed Literatures','Side Bar menu page',1

EXEC [dbo].pInsTranslation 'Document_Created_From',N'تم إنشاء المستند من','Document Created From','Legal Document Advance Search page',1
EXEC [dbo].pInsTranslation 'Document_Created_To',N'تم إنشاء المستند','Document Created To','Legal Document Advance Search page',1

EXEC [dbo].pInsTranslation 'Create_Principle_Mask',N'انشاء التظليل للمبدأ','Create Principle Mask','Legal Principle Masked',1
EXEC [dbo].pInsTranslation 'Edit_Principle_Mask',N'تعديل التظليل للمبدأ','Edit Principle Mask','Legal Principle Masked',1
EXEC [dbo].pInsTranslation 'Search_Principle_Number',N'ابحث عن رقم المبدأ','Search Principle Number','Legal Principle Masked',1
EXEC [dbo].pInsTranslation 'Principle_Details',N'تفاصيل المبدأ','Principle Details','Legal Principle Masked',1
EXEC [dbo].pInsTranslation 'Masked_Principle_View',N'عرض المبدأ المظلل','Masked Principle View','Legal Principle Masked',1

EXEC [dbo].pInsTranslation 'Edit_Literature_Type',N'تعديل نوع الكتاب','Edit Literature Type','Side Bar menu page',1

EXEC [dbo].pInsTranslation 'UnPublish_Confirm',N'هل أنت متأكد من إلغاء النشر؟','Are you sure you want to Unpublish?','Publish UnPublish Document page ',1

EXEC [dbo].pInsTranslation 'BarCodeNumber',N'رقم الباركود','Barcode Number','Lms Literature Borrow Detail Add/Edit page',1

EXEC [dbo].pInsTranslation 'Legal_Principles_Review',N'مراجعة المبادئ القانونية','Legal Principles Review','Legal Principles Review page',1

EXEC [dbo].pInsTranslation 'Arabic_Name_Is_Allowed',N'مسموح بالاسم العربي فقط','Only Arabic Name Is Allowed','Add LMS literature Page',1
EXEC [dbo].pInsTranslation 'Upload_Invalid_Field_Message',N'تحميل ملف صالح','Upload Valid File','Document Catalogue Add',1

EXEC [dbo].pInsTranslation 'Add_Catalogue_Page',N'اضافة فهرس المستندات القانونية',' Add Legal Document Catalogue.','Add Lds Document Catalogue page',1

EXEC [dbo].pInsTranslation 'Catalogue_Is_Disabled',N'غير نشط','InActive','Add Lds Document Catalogue page',1

EXEC [dbo].pInsTranslation 'most_active_member',N'العضو الأكثر نشاطا','Most Active Member','Dashboard',1
EXEC [dbo].pInsTranslation 'Legal_Libary',N'المكتبة القانونية','Legal Library','Main layout topbar menu',1
EXEC [dbo].pInsTranslation 'Law_Managment',N'ادارة القانون','Law Managment','Main layout topbar menu',1
EXEC [dbo].pInsTranslation 'Manage_Consultation',N'إدارة الاستشارة','Manage Consultation','Main layout topbar menu',1
EXEC [dbo].pInsTranslation 'User_Managment',N'إدارة المستخدم','User Managment','Main layout topbar menu',1
EXEC [dbo].pInsTranslation 'Tags',N'الوسوم','Tags','Add literature wizard step ',1
EXEC [dbo].pInsTranslation 'Book_Number',N'رقم الكتاب','Book Number','LMS  add literature Detail page ',1	
EXEC [dbo].pInsTranslation 'BorrowerDetails',N'الكتب المستعارة','Borrowed Literature','LmsLiteratureBorrowDetail page',1
EXEC [dbo].pInsTranslation 'View_by',N'آلية البحث','Search Criteria','Literature Borrow Approval page',1
EXEC [dbo].pInsTranslation 'Choose_Search_Item',N'البحث عن','Search For','Lms Literature Index Divisions page',1
EXEC [dbo].pInsTranslation 'Literature_borrow_extension_approval',N'تمديد فترة استعارة الكتاب','Literature Borrow Extension','Literature borrow extension approval page',1 
EXEC [dbo].pInsTranslation 'Literature_Borrow_Approval',N'مراجعة طلبات الاستعارة','Review Borrow Request','Literature Borrow Approval page',1
EXEC [dbo].pInsTranslation 'Associated_Document_To_Catalog_Title',N'الوثائق المرفقة','Associated Document','Lds Document Catalogue view page',1
EXEC [dbo].pInsTranslation 'Select_Files',N'اختيار الملفات','Select Files','Lds  edit Document Catalogue  page',1
EXEC [dbo].pInsTranslation 'Reviewer_Name',N'اسم المراجع','Reviewer Name','Legal Principle',1
EXEC [dbo].pInsTranslation 'Review_Date',N'تاريخ المراجعة','Review Date','Legal Principle',1
EXEC [dbo].pInsTranslation 'Update',N'تحديث','Update','Legal Principle hierarchy add ',1
EXEC [dbo].pInsTranslation 'Catagory_Nmae',N'التصنيف','Category ','Legal Principle',1
EXEC [dbo].pInsTranslation 'User',N'المستخدم','User ','Legal Principle',1
EXEC [dbo].pInsTranslation 'Legal_Principles_Approval',N'مراجعة المبادئ','Review Principles','Side Bar menu page',1
EXEC [dbo].pInsTranslation 'Legal_Principles_Publish_and_UnPublish',N'نشر/إلغاء نشر المبادئ','Publish/UnPublish Principles','Side Bar menu page',1
EXEC [dbo].pInsTranslation 'Legal_Principles_Masked',N'المبادئ المظللة','Masked Principle','Side Bar menu page',1
EXEC [dbo].pInsTranslation 'Case_Managment',N'ادارة القضايا','Case Managment','Case Request page',1
EXEC [dbo].pInsTranslation 'Borrow_Approval_Status',N'حالة الموافقة على الاعارة','Borrow Approval Status','AddLmsLiteratureBorrowDetail page',1
EXEC [dbo].pInsTranslation 'Case_Request',N'طلب قضية','Case Request','Case Request page',1
EXEC [dbo].pInsTranslation 'Request_Number',N'رقم الطلب','Request Number','Case Request page',1
EXEC [dbo].pInsTranslation 'Case_Request_Status',N'حالة طلب القضية','Case Request Status','Case Request page',1
EXEC [dbo].pInsTranslation 'Priority',N'الأولوية','Priority','Case Request page',1
EXEC [dbo].pInsTranslation 'Governament_Name',N'اسم الجهة الحكومية','Government Name','Case Request page',1
EXEC [dbo].pInsTranslation 'Legal_Principle_Tags',N'وسوم المبادئ القانونية','Legal Principle Tags','Legal Principle Tags page',1
EXEC [dbo].pInsTranslation 'Principle_Tag_number_is_associated_with',N'المبدأ رقم العلامة مرتبط بـ','Principle Tag number is associated with','Legal Principle Tags page',1
EXEC [dbo].pInsTranslation 'Principle_Tag_with_Principle',N'علامة المبدأ مع المبدأ','Principle Tag with Principle','Legal Principle Tags page',1




EXEC [dbo].pInsTranslation 'IP_Details',N'تفاصيل الـIP','IP Details','Case Request page',1


EXEC [dbo].pInsTranslation 'Case_Request_Information',N'قسم معلومات طلب القضية','Case Request Information Section','Case Request page',1
EXEC [dbo].pInsTranslation 'Case_Requests_Detail',N'بيانات طلبات القضايا','Case Requests Detail','Case Request Detail page',1
EXEC [dbo].pInsTranslation 'Case_Type',N'نوع القضية','Case Type','Case Request Detail page',1
EXEC [dbo].pInsTranslation 'Case_Requested_Date',N'التاريخ المطلوب للقضية','Case Requested Date','Case Request Detail page',1
EXEC [dbo].pInsTranslation 'Request_Number',N'رقم الطلب','Request Number','Case Request Detail page',1
EXEC [dbo].pInsTranslation 'GE_User',N'مستخدم الجهة الحكومية','GE User','Case Request Detail page',1
EXEC [dbo].pInsTranslation 'Defendant_Type',N'نوع المدعى عليه','Defendant Type','Case Request Detail page',1
EXEC [dbo].pInsTranslation 'Defendant_Name',N'اسم المدعى عليه','Defendant Name','Case Request Detail page',1
EXEC [dbo].pInsTranslation 'Defendant_Civil_ID',N'الرقم المدني للمدعى عليه','Defendant Civil ID','Case Request Detail page',1
EXEC [dbo].pInsTranslation 'Commercial_Registration_Number',N'رقم السجل التجاري','Commercial Registration Number','Case Request Detail page',1
EXEC [dbo].pInsTranslation 'Request_Created_by',N'تم إنشاء الطلب عن طريق','Request Created by','Case Request Detail page',1
EXEC [dbo].pInsTranslation 'Request_Created_Date',N'طلب تاريخ الإنشاء','Request Created Date','Case Request Detail page',1
EXEC [dbo].pInsTranslation 'Request_Modified_By',N'تم تعديل الطلب عن طريق','Request Modified By','Case Request Detail page',1
EXEC [dbo].pInsTranslation 'Request_Modified_Date',N'تاريخ تعديل الطلب','Request Modified Date','Case Request Detail page',1


EXEC [dbo].pInsTranslation 'Registerd_Requests',N'الطلبات المسجلة','Registered Requests','Registered Requests page',1
EXEC [dbo].pInsTranslation 'File_Number',N'رقم الملف','File Number','Registerd Requests page',1

EXEC [dbo].pInsTranslation 'Principle_Information_Section',N'قسم معلومات المبدأ','Principle Information Section','Principle Detail Requests page',1
EXEC [dbo].pInsTranslation 'Principle_Reference_Content_Section',N'قسم محتوى مرجع المبدأ','Principle Reference Content Section','Principle Detail Requests page',1
EXEC [dbo].pInsTranslation 'Principle_Content_Section',N'قسم محتوى المبدأ','Principle Content Section','Principle Detail Requests page',1
EXEC [dbo].pInsTranslation 'Under_Filing_Case_Section',N'قسم قضايا تحت الرفع','Under Filing Case Section','Principle Detail Requests page',1

EXEC [dbo].pInsTranslation 'Case_Registerd_Requests_Detail',N'تفاصيل الطلبات المسجلة بالقضية','Case Registered Requests Detail','Registered Requests Detail page',1
EXEC [dbo].pInsTranslation 'FileName_En',N'اسم الملف باللغة الانجليزية','FileName_En','Registerd Requests Detail page',1
EXEC [dbo].pInsTranslation 'FileName_Ar',N'اسم الملف باللغة العربية','FileName_Ar','Registerd Requests Detail page',1
EXEC [dbo].pInsTranslation 'File_Date',N'تاريخ الملف','File_Date','Registerd Requests Detail page',1
EXEC [dbo].pInsTranslation 'Request_Number',N'رقم الطلب','Request Number','Registerd Requests Detail page',1
EXEC [dbo].pInsTranslation 'CaseRequested_Date',N'تاريخ القضية المطلوبة','CaseRequested Date','Registerd Requests Detail page',1
EXEC [dbo].pInsTranslation 'Case_TypeEn',N'نوع القضية باللغة الانجليزية','Case Type (English)','Registerd Requests Detail page',1
EXEC [dbo].pInsTranslation 'Case_TypeAr',N'نوع القضية باللغة العربية','Case Type (Arabic)','Registerd Requests Detail page',1
EXEC [dbo].pInsTranslation 'Request_Description',N'وصف الطلب','Request Description','Registerd Requests Detail page',1
EXEC [dbo].pInsTranslation 'Reference_Number',N'رقم المرجع','Reference Number','Registerd Requests Detail page',1
EXEC [dbo].pInsTranslation 'GEUName_Ar',N'اسم الجهة الحكومية باللغة العربية','Governament Entity Name (Arabic)','Registerd Requests Detail page',1
EXEC [dbo].pInsTranslation 'GEUName_En',N'اسم الجهة الحكومية باللغة الانجليزية','Governament Entity Name (English)','Registerd Requests Detail page',1
EXEC [dbo].pInsTranslation 'Case_Request_Status',N'حالة طلب القضية','Case Request Status','Registerd Requests Detail page',1
EXEC [dbo].pInsTranslation 'User_ID',N'رقم المستخدم','User ID','Registerd Requests Detail page',1
EXEC [dbo].pInsTranslation 'Is_Confidential',N'هل يتصف بالسرية','Is Confidential','Registerd Requests Detail page',1
EXEC [dbo].pInsTranslation 'Case_Party_Catagory',N'تصنيف طرف القضية','Case Party Catagory','Registerd Requests Detail page',1
EXEC [dbo].pInsTranslation 'Party_type',N'نوع الطرف','Party type','Registerd Requests Detail page',1
EXEC [dbo].pInsTranslation 'Case_Party_Link_Name_En',N'اسم رابط طرف القضية باللغة العربية','Case Party Link Name (English)','Registerd Requests Detail page',1
EXEC [dbo].pInsTranslation 'Case_Party_LinkName_Ar',N'اسم رابط طرف القضية باللغة الانجليزية','Case Party Link Name (Arabic)','Registerd Requests Detail page',1
EXEC [dbo].pInsTranslation 'Civil_ID',N'الرقم المدني','Civil ID','Registerd Requests Detail page',1
EXEC [dbo].pInsTranslation 'Address',N'العنوان','Address','Registerd Requests Detail page',1
EXEC [dbo].pInsTranslation 'UserName',N'اسم المستخدم','UserName','Registerd Requests Detail page',1
EXEC [dbo].pInsTranslation 'Recived_By',N'تم الاستلام عن طريق','Recived By','Registerd Requests Detail page',1
EXEC [dbo].pInsTranslation 'Aproved_By',N'تمت الموافقة عن طريق','Aproved By','Registerd Requests Detail page',1


EXEC [dbo].pInsTranslation 'Response_Comments',N'التعليقات','Comments','Registerd Requests Detail page',1
EXEC [dbo].pInsTranslation 'Response_Justification',N'التبرير','Justification','Registerd Requests Detail page',1
EXEC [dbo].pInsTranslation 'Response_Due_Date',N'تاريخ الانتهاء','Due Date','Registerd Requests Detail page',1
EXEC [dbo].pInsTranslation 'Is_Urgent',N'هل هو مستعجل','Is Urgent','Registerd Requests Detail page',1
EXEC [dbo].pInsTranslation 'Response_Detail_Section',N'قسم تفاصيل الرد','Response Detail Section','Registerd Requests Detail page',1
EXEC [dbo].pInsTranslation 'Response_Details',N'تفاصيل الرد','Response Details','Registerd Requests Detail page',1
EXEC [dbo].pInsTranslation 'View_Response',N'عرض الرد','View Response','Registerd Requests Detail page',1
EXEC [dbo].pInsTranslation 'Other_Reason',N'أسباب أخرى','Other Reason','Registerd Requests Detail page',1
EXEC [dbo].pInsTranslation 'Reponse_Id',N'رقم الرد','Reponse Id','Registerd Requests Detail page',1


EXEC [dbo].pInsTranslation 'User_Transfer_Detail',N'بيانات نقل المستخدم','User Transfer Detail','Ums Transfer User Details page',1
EXEC [dbo].pInsTranslation 'User_Transfer_History',N'تاريخ نقل المستخدم','User Transfer History','Ums Transfer History page',1
EXEC [dbo].pInsTranslation 'Details_and_Desicions',N'القرارات والتفاصيل','Details and Desicions','Lds Document Aprove Reject page',1

EXEC [dbo].pInsTranslation 'Principle_Title_Edit',N'تعديل المبدأ','Edit Principle','Legal Principle Add',1
EXEC [dbo].pInsTranslation 'Only_English_Character_allowed',N'مسموح استخدام الحروف باللغة الإنجليزية فقط','Only English Character allowed','Document Catalogue Add',1
EXEC [dbo].pInsTranslation 'Is_Empty',N'فارغ','Is Empty','Grid Filter Is Empty',1


EXEC [dbo].pInsTranslation 'Literature_Details',N'تفاصيل الكتاب','Literature Details','LMS literature Detail page ',1
EXEC [dbo].pInsTranslation 'Literature_Type_Name_En',N'اسم نوع الكتاب (انجليزي)','Literature Type Name (English)','LMS literature Detail page ',1
EXEC [dbo].pInsTranslation 'Type_Name',N'أكتب اسم','Type Name','LMS literature Detail page ',1
EXEC [dbo].pInsTranslation 'Literature_Type_Name_Ar',N'اسم نوع الكتاب (عربي)','Literature Type Name (Arbaic)','LMS literature Detail page ',1
EXEC [dbo].pInsTranslation 'Literature_Author_First_Name_En',N'الاسم الأول للمؤلف (انجليزي)','Literature Author First Name (English)','LMS literature Detail page ',1
EXEC [dbo].pInsTranslation 'Literature_Author_First_Name_Ar',N'(100) الاسم الأول للمؤلف (عربي)','Literature Author First Name (Arbaic)(100)','LMS literature Detail page ',1
EXEC [dbo].pInsTranslation 'Literature_Author_Last_Name_En',N'الاسم الأخير للمؤلف (انجليزي)','Literature Author Last Name (English)','LMS literature Detail page ',1
EXEC [dbo].pInsTranslation 'Literature_Author_Last_Name_Ar',N'الاسم الأخير للمؤلف (عربي)','Literature Author Last Name (Arbaic)','LMS literature Detail page ',1    
EXEC [dbo].pInsTranslation 'Literature_Classification_Name_En',N'اسم تصنيف الكتاب (انجليزي)','Classification Last Name (English)','LMS literature Detail page ',1
EXEC [dbo].pInsTranslation 'Literature_Classification_Name_Ar',N'اسم تصنيف الكتاب (عربي)','Classification Last Name (Arbaic)','LMS literature Detail page ',1
EXEC [dbo].pInsTranslation 'Character90',N'90 حرف','Character 90','LMS literature Detail page ',1    
EXEC [dbo].pInsTranslation 'Character82',N'82 حرف','Character 82','LMS literature Detail page ',1     
EXEC [dbo].pInsTranslation 'Character92',N'92 حرف','Characters 92','LMS literature Detail page ',1    
EXEC [dbo].pInsTranslation 'Publisher',N'الناشر','Publisher','LMS literature Detail page ',1 
EXEC [dbo].pInsTranslation 'Tag_Section',N'قسم الوسوم','Tag Section','LMS literature Detail page ',1    
EXEC [dbo].pInsTranslation 'Literature_Information',N'قسم معلومات الكتاب','Literature Information Section','LMS literature Detail page ',1
EXEC [dbo].pInsTranslation 'Author_Information',N'قسم معلومات المؤلف','Author Information Section','LMS literature Detail page ',1
EXEC [dbo].pInsTranslation 'Index_Information',N'معلومات الفهرس','Index Information','LMS literature Detail page ',1    
EXEC [dbo].pInsTranslation 'Devision_Information',N'معلومات القسم','Devision Information','LMS literature Detail page ',1    
EXEC [dbo].pInsTranslation 'Literature_and_Index_Division_Information',N'فهرس الكتاب ومعلومات القسم','Literature  Index And  Division Information Section','LMS literature Detail page ',1  
EXEC [dbo].pInsTranslation 'Parent_Index_Number',N'فهرس الكتاب الرئيسي','Parent Index Number','Add LMS index page',1
EXEC [dbo].pInsTranslation 'Document_Version_Restore_Successfully',N'لقد تمت استعادة إصدار الوثيقة بنجاح','Document Version Restored Successfully','General',1
EXEC [dbo].pInsTranslation 'Is_Not_Empty_Text',N'ليس فارغا','Is Not Empty','Log In State',1
EXEC [dbo].pInsTranslation 'Is_Empty_Text',N'فارغ','Is Empty','Log In State',1
EXEC [dbo].pInsTranslation 'Index_Number_Range',N'يجب أن يتكون رقم الفهرس من 3 أرقام','Index number must be 3 digits','Add Lms Literature Index  page ',1
EXEC [dbo].pInsTranslation 'Catalog_Information',N'معلومات فهرس الوثائق','Catalog Information','Document Catalog',1
EXEC [dbo].pInsTranslation 'Select_Principle',N'اختر المبدأ','Select Principle','Legal Principle Hierarchy',1


--- Dashboard
EXEC [dbo].pInsTranslation 'Total_Legal_Documents',N'مجموع المستندات القانونية','Total Legal Documents','Dashboard',1
EXEC [dbo].pInsTranslation 'Total_Legal_Principles',N'مجموع المبادئ القانونية','Total Legal Principles','Dashboard',1
---
EXEC [dbo].pInsTranslation 'Sure_Delete_Legal_Principle',N'تأكد من أنك تريد حذف المبدأ القانوني','Sure You Want To Delete Legal Principle','Legal Principle',1
EXEC [dbo].pInsTranslation 'StartFrom',N'يبدأ من','Start From','Legal Legislation',1
EXEC [dbo].pInsTranslation 'EndTo',N'ينتهي في','End To','Legal Legislation',1
EXEC [dbo].pInsTranslation 'Unable_Delete_Legislation',N'تعذر حذف التشريع','Unable Delete Legislation','Legal Legislation',1
EXEC [dbo].pInsTranslation 'Reference_With_Legal_Legislation',N'المرجع مع التشريع القانوني','Reference With Legal Legislation','Legal Legislation',1
EXEC [dbo].pInsTranslation 'Clear',N'مسح','Clear','Legal Legilation',1
EXEC [dbo].pInsTranslation 'Sure_Revoke_Delete_Legislation',N'تأكد من إلغاء حذف التشريع','Sure Revoke Delete Lesgislation','Legal Legislation',1 
EXEC [dbo].pInsTranslation 'Legal_Legislation_Review',N'مراجعة التشريعات','Review Legislations','Side Bar menu page',1
EXEC [dbo].pInsTranslation 'LegalLegislation',N'التشريعات القانونية','Legal Legislations','Side Bar menu page',1 
EXEC [dbo].pInsTranslation 'Legal_Legislation_Approved',N'التشريعات المعتمدة','Approved Legislations','Side Bar menu page',1
EXEC [dbo].pInsTranslation 'Legal_Legislation_Publish_and_UnPublish',N'نشر/إلغاء نشر التشريع','Publish/Unpublish Legislations','Side Bar menu page',1
EXEC [dbo].pInsTranslation 'Legal_Legislation_Delete',N'التشريعات المحذوفة','Deleted Legislations','Side Bar menu page',1
EXEC [dbo].pInsTranslation 'End_Level',N'هذا هو المستوى النهائي الذي لا يمكن إنشاء مستوى فرعي آخر','This is the end level no further child can be created','LMS literature Index page ',1
EXEC [dbo].pInsTranslation 'Principle_Add_New_Relation_Save_Popup_Confirm_Message',N'هل أنت متأكد أنك تريد الإلغاء؟','Are you sure you want to cancel?','Legal Principle Add Page',1
