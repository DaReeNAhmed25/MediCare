# **MediCare — المواصفات الموحدة**

Nov 18, 2026 · @Ahmed

## **1\. القرارات المعتمدة**

اعتمدنا 11 قرارًا لحل التناقضات بين خطة المشروع ومستند التصميم، وهذا المستند هو المرجع الوحيد من الآن.

| الموضوع | القرار | أثره |
| ----- | ----- | ----: |
| التنبيهات | إيميل فقط، بدون SignalR | حذف Azure SignalR Service وUS-18 والخطر R14 |
| مزود الإيميل | SendGrid | تعديل NFR-08 وC-03 ومخطط النشر؛ المفتاح في Azure App Settings |
| صلاحيات الأدمن | يؤكد ويرفض ويلغي أي موعد، ولا يوجد Walk-in | حذف FR-36؛ يصبح Patients.UserId إلزاميًا |
| السجل الطبي | بدون توقيع أو قفل أو Addenda | حفظ الزيارة يجعل الموعد Completed مباشرة |
| AuditLogs | تأكيد ورفض وإلغاء فقط | لا يُسجَّل عرض السجلات |
| حذف المستخدم | إضافة Users.IsDeleted | الحساب المحذوف لا يسجل الدخول |
| CI/CD | GitHub Actions | بدون Azure DevOps |
| تغيير ساعات العمل | مرفوض إذا وُجدت مواعيد حالية | تُعرض قائمة المواعيد للطبيب |
| الجدول | 3 Sprints من 18 إلى 30 نوفمبر | بدلًا من 4 Sprints |
| ما يُقص أولًا | تصدير PDF/Excel، ثم الوضع الداكن، ثم إيميل التذكير | حسب ترتيب الخطر R2 |
| التصاميم | موجودة في Figma | يُضاف الرابط في القسم 7 |

## **2\. النطاق**

MediCare منصة ويب لإدارة العيادة والمواعيد على ASP.NET Core MVC وEF Core وASP.NET Identity، بثلاثة أدوار (مريض، طبيب، أدمن)، وتُنشر على Azure.

| داخل النطاق | خارج النطاق |
| ----: | ----- |
| ثلاثة أدوار وتفعيل الحساب بـ OTP عبر الإيميل للمريض والطبيب | التكامل مع أنظمة المستشفيات أو السجلات الصحية |
| ملفات الأطباء والبحث بالتخصص والموقع والتوفر | التشخيص بالذكاء الاصطناعي |
| الحجز والإلغاء وإعادة الجدولة مع منع الحجز المزدوج | الدفع الإلكتروني والفواتير والتأمين |
| تأكيد الموعد أو رفضه من الطبيب أو الأدمن | SMS والتنبيهات الفورية SignalR |
| زيارات طبية ووصفات رقمية قابلة للطباعة | التحقق الآلي من رخصة الطبيب |
| إيميلات عبر SendGrid (OTP، حجز، تأكيد، رفض، إلغاء، تذكير) | تسجيل مرضى Walk-in وحجز الأدمن نيابة عنهم |
| لوحة الأدمن والتقارير اليومية وإدارة الأطباء والمرضى والتخصصات | توقيع السجل الطبي وقفله وسجل عرض السجلات |
| تصدير PDF/Excel (Should) | بيانات مرضى حقيقية وتطبيقات Android/iOS |

## **3\. حالات الاستخدام**

هناك 11 حالة استخدام وثلاثة ممثلين؛ SendGrid نظام خارجي يستقبل الطلبات ولا يبدأ أي تفاعل.

| الرقم | الحالة | الممثلون | القاعدة النهائية |
| ----- | ----: | ----: | ----: |
| UC-01 | التسجيل وتأكيد الإيميل | مريض، طبيب | حساب غير نشط حتى إدخال OTP من 6 أرقام صالح 10 دقائق، وقفل بعد 5 محاولات خاطئة |
| UC-02 | تسجيل الدخول | الكل | الحساب غير المؤكد أو الموقوف أو المحذوف لا يدخل |
| UC-03 | البحث عن طبيب | مريض | بالتخصص والموقع والتوفر، ولا يتغير أي بيان |
| UC-04 | حجز موعد | مريض | يُنشأ Pending في Transaction واحدة ثم يُرسل إيميل للمريض والطبيب |
| UC-05 | إلغاء أو إعادة جدولة | مريض (إلغاء وإعادة جدولة)، أدمن (إلغاء) | قبل الموعد بساعتين على الأقل؛ الموعد المعاد جدولته يعود Pending |
| UC-06 | ساعات العمل والإجازات | طبيب | يُرفض تغيير الساعات أو طول الموعد أو الإجازة إذا تعارض مع مواعيد حالية، وتُعرض قائمتها |
| UC-07 | تأكيد أو رفض طلب | طبيب (مواعيده)، أدمن (الكل) | أول قرار يُحفظ، ومن يأتي بعده يرى الحالة المحدثة |
| UC-08 | تسجيل زيارة وإصدار وصفة | طبيب | موعد Confirmed فقط؛ علامات حيوية وأعراض وتشخيص وملاحظات ووصفة اختيارية، والحفظ يجعل الموعد Completed |
| UC-09 | عرض السجل الطبي | مريض | سجلاته ووصفاته فقط، وطلب سجل غيره يُرفض |
| UC-10 | التقرير اليومي واللوحة | أدمن | تسجيلات وحجوزات وإلغاءات ليوم محدد، ورسوم Chart.js |
| UC-11 | إدارة الأطباء والمرضى والتخصصات | أدمن | حذف ناعم فقط؛ تخصص مستخدم لا يُحذف، ويُعرض الإيقاف أو نقل أطبائه أولًا |

## **4\. المتطلبات الوظيفية**

هناك 35 متطلبًا وظيفيًا: 30 Must و5 Should (FR-24 وFR-25 وFR-27 وFR-28 وFR-35). حُذف FR-36 (Walk-in)، وأُبقيت الأرقام الأخرى كما في مستند التصميم لسهولة التتبع.

| الرقم | المجال | يجب أن يقوم النظام بـ | الأولوية |
| ----- | ----: | ----- | ----- |
| FR-01 | الحسابات | دعم ثلاثة أدوار: مريض وطبيب وأدمن | Must |
| FR-02 | الحسابات | تسجيل المريض أو الطبيب بالاسم والإيميل وكلمة المرور وبيانات الدور (طبيب: تخصص وموقع وسعر وصورة؛ مريض: بيانات شخصية وحساسية وتاريخ مرضي) | Must |
| FR-03 | الحسابات | إرسال OTP من 6 أرقام عند التسجيل، وتفعيل الحساب بعد إدخاله صحيحًا فقط | Must |
| FR-04 | الحسابات | OTP صالح 10 دقائق ومخزن مُشفَّرًا (Hash)، ويُقفل بعد 5 محاولات خاطئة، ويُطلب غيره بعد فاصل قصير | Must |
| FR-05 | الحسابات | تسجيل الدخول والخروج بالإيميل وكلمة المرور، وتقييد كل صفحة بالأدوار المسموح لها | Must |
| FR-06 | الحسابات | إنشاء حساب الأدمن عند تهيئة النظام وليس بالتسجيل العام | Must |
| FR-07 | الأطباء | إدراج الطبيب في الدليل بعد اكتمال التسجيل وتأكيد الإيميل وملفه | Must |
| FR-08 | الأطباء | عرض التخصص والسعر والموقع والصورة والمواعيد المتاحة في ملف الطبيب | Must |
| FR-09 | الأطباء | بحث وتصفية الأطباء بالتخصص والموقع والتوفر | Must |
| FR-10 | الأطباء | تعديل الطبيب ملفه الشخصي | Must |
| FR-11 | المواعيد | ضبط الطبيب ساعات العمل الأسبوعية وأيام الإجازة | Must |
| FR-12 | المواعيد | عرض المواعيد الحرة فقط (داخل ساعات العمل، خارج الإجازات، غير محجوزة) في تقويم تفاعلي | Must |
| FR-13 | المواعيد | حجز المريض موعدًا حرًا مع أي طبيب | Must |
| FR-14 | المواعيد | منع الحجز المزدوج حتى لو حجز مريضان في اللحظة نفسها | Must |
| FR-15 | المواعيد | تتبع الحالة: Pending وConfirmed وRejected وCancelled وCompleted | Must |
| FR-16 | المواعيد | إلغاء المريض موعده أو إعادة جدولته (قبل ساعتين على الأقل) وتحرير الموعد القديم، وإلغاء الأدمن أي موعد | Must |
| FR-17 | المواعيد | تأكيد الطبيب (لمواعيده) أو الأدمن (للكل) الطلبات المعلقة أو رفضها | Must |
| FR-18 | المواعيد | عرض الطبيب مواعيده اليومية والأسبوعية في تقويم وقائمة | Must |
| FR-19 | المواعيد | عرض المريض مواعيده القادمة والسابقة | Must |
| FR-20 | السجلات | تسجيل الطبيب علامات حيوية وأعراضًا وتشخيصًا وملاحظات علاج، وحفظها يجعل الموعد Completed | Must |
| FR-21 | السجلات | إنشاء وصفة رقمية بالدواء والجرعة والتكرار | Must |
| FR-22 | السجلات | عرض وصفة قابل للطباعة | Must |
| FR-23 | السجلات | عرض المريض سجله ووصفاته هو فقط | Must |
| FR-24 | السجلات | تحديث المريض بياناته وحساسيته وتاريخه المرضي | Should |
| FR-25 | السجلات | فتح الطبيب بيانات مرضاه الذين لديهم موعد معه | Should |
| FR-26 | الإشعارات | إرسال إيميل عبر SendGrid عند OTP والحجز والتأكيد والرفض والإلغاء وإعادة الجدولة | Must |
| FR-27 | الإشعارات | إيميل تذكير قبل الموعد المؤكد بـ 24 ساعة | Should |
| FR-28 | الإشعارات | سجل لكل إيميل مُرسل (النوع والمستلم والحالة والوقت) لإعادة إرسال الفاشل | Should |
| FR-29 | الأدمن | صفحة أدمن خاصة بدور الأدمن فقط | Must |
| FR-30 | الأدمن | تقرير يومي لتاريخ محدد: تسجيلات جديدة وحجوزات ومؤكدة وملغاة | Must |
| FR-31 | الأدمن | لوحة بمخططات المواعيد الشهرية والأطباء والتخصصات الأكثر طلبًا واتجاه المرضى | Must |
| FR-32 | الأدمن | عرض الأطباء والمرضى وتعديلهم وإيقافهم وحذفهم حذفًا ناعمًا | Must |
| FR-33 | الأدمن | إضافة التخصصات وتعديلها وحذفها (غير المستخدم فقط) | Must |
| FR-34 | الأدمن | عرض كل المواعيد وتأكيدها أو رفضها أو إلغاؤها | Must |
| FR-35 | الأدمن | تصدير التقارير إلى PDF أو Excel | Should |

## **5\. المتطلبات غير الوظيفية والقيود**

هناك 19 متطلبًا غير وظيفي بالأرقام نفسها في المستندين، والتغيير الوحيد في NFR-08 (SendGrid بدل Gmail). وهناك 7 قيود بعد تحديث C-03 وC-06.

| الرقم | الفئة | المتطلب | المقياس |
| ----- | ----: | ----: | ----- |
| NFR-01 | الأداء | سرعة الصفحات | 95% من الطلبات أقل من ثانيتين |
| NFR-02 | الأداء | سرعة لوحة الأدمن | أقل من 5 ثوانٍ مع 10,000 موعد تجريبي |
| NFR-03 | الأداء | وصول إيميل OTP | 95% خلال دقيقة |
| NFR-04 | الأمن | كلمات المرور | مُشفَّرة (Hash) بواسطة ASP.NET Identity |
| NFR-05 | الأمن | التشفير | الموقع عبر HTTPS فقط |
| NFR-06 | الأمن | الصلاحيات على الخادم | من يفتح سجل غيره بالرابط يُرفض |
| NFR-07 | الأمن | مقاومة الهجمات | تحقق من المدخلات وAnti-forgery وترميز المخرجات واستعلامات EF Core |
| NFR-08 | الأمن | الأسرار خارج الكود | مفتاح SendGrid وسلسلة الاتصال في Azure App Settings وليس GitHub |
| NFR-09 | الخصوصية | موافقة المستخدم | قبول إشعار الخصوصية عند التسجيل |
| NFR-10 | سهولة الاستخدام | الهواتف وسطح المكتب | Bootstrap 5 متجاوب |
| NFR-11 | سهولة الاستخدام | حجز بلا مساعدة | أقل من دقيقتين في اختبار القبول |
| NFR-12 | سهولة الاستخدام | إمكانية الوصول | Lighthouse 90 فأكثر؛ الوضع الداكن Should |
| NFR-13 | سهولة الاستخدام | أخطاء واضحة | رسالة بجوار كل حقل فاشل |
| NFR-14 | الاعتمادية | التوفر | 99% خلال فترة التقييم |
| NFR-15 | الاعتمادية | الحفظ المتزامن | الحجز والإلغاء والوصفة في Unit of Work واحدة |
| NFR-16 | الاعتمادية | الأخطاء | صفحة خطأ ودية مع تسجيل الخطأ |
| NFR-17 | الصيانة | فصل الطبقات | Web وServices وData وRepository وUnit of Work وDI |
| NFR-18 | الصيانة | التتبع | فروع ميزات وPull Requests وREADME |
| NFR-19 | التوافق | المتصفحات | آخر إصدار من Chrome وEdge وFirefox |

**القيود**

* C-01: ASP.NET Core MVC وEF Core وIdentity بمعمارية N-Tier وأنماط Repository وUnit of Work وFactory وDI.  
* C-02: النشر على Azure App Service وAzure SQL مع CI/CD عبر GitHub Actions.  
* C-03: الإيميل عبر SendGrid هو القناة الخارجية الوحيدة؛ لا SMS ولا تنبيهات فورية ولا دفع إلكتروني.  
* C-04: بيانات تجريبية فقط، بلا بيانات مرضى حقيقية.  
* C-05: تطبيق ويب فقط.  
* C-06: 12 يومًا للتنفيذ (18 إلى 30 نوفمبر) في 3 Sprints، ثم الاختبار والتقارير حتى 4 ديسمبر.  
* C-07: فريق من خمسة أعضاء عبر GitHub بفروع وPull Requests.

## **6\. قواعد العمل**

هناك 11 قاعدة؛ حُذفت قاعدة التوقيع (BR-06 القديمة) وأُضيفت قاعدتا ساعات العمل والحذف.

| الرقم | القاعدة |
| ----- | ----- |
| BR-01 | الموعد الواحد يحمل حجزًا نشطًا واحدًا على الأكثر (أي حالة غير Cancelled وRejected). |
| BR-02 | الحجز داخل نافذة ساعات الطبيب، ويتوافق مع طول الموعد SlotMinutes، وليس في يوم إجازة. |
| BR-03 | الحجز الجديد يبدأ Pending؛ يؤكده أو يرفضه الطبيب (لمواعيده) أو الأدمن (لأي موعد)، وأول قرار هو المعتمد. |
| BR-04 | يُلغى الموعد Pending أو Confirmed أو يُعاد جدولته قبل بدئه بساعتين على الأقل (قابلة للضبط)، والموعد المعاد جدولته يعود Pending. |
| BR-05 | يُنشأ السجل الطبي لموعد Confirmed فقط، وحفظه يجعل الموعد Completed في Transaction واحدة. |
| BR-06 | لا يُغيَّر جدول الطبيب (ساعات أو طول موعد أو إجازة) إذا تعارض مع مواعيد Pending أو Confirmed قادمة، وتُعرض قائمتها. |
| BR-07 | المرضى والأطباء والتخصصات يُحذفون حذفًا ناعمًا (IsDeleted)، وحذف مستخدم يمنعه من الدخول؛ تخصص مستخدم لا يُحذف؛ السجلات الطبية وAuditLogs لا تُحذف. |
| BR-08 | كل تأكيد ورفض وإلغاء يكتب سجلًا في AuditLogs. |
| BR-09 | المريض والطبيب يؤكدان إيميلهما بـ OTP قبل تفعيل الحساب؛ الأدمن يُنشأ عند التهيئة. |
| BR-10 | SendGrid هو قناة الإشعار الوحيدة، وكل إيميل يُسجَّل في جدول Notifications. |
| BR-11 | لا يرى المريض إلا سجلاته، ولا يرى الطبيب إلا مرضاه الذين لديهم موعد معه. |

## **7\. البنية والتقنيات**

النظام N-Tier من ثلاثة مشاريع تُنشر معًا كتطبيق واحد على Azure App Service، والاعتماد في اتجاه واحد: Web ثم Services ثم Data.

| الطبقة | المسؤولية | المحتوى |
| ----- | ----: | ----- |
| MediCare.Web | استقبال الطلبات والتحقق من الدور وعرض الصفحات، بلا قواعد عمل | Controllers وRazor Views وViewModels وIdentity وBootstrap 5 وFullCalendar وChart.js |
| MediCare.Services | كل قواعد العمل: حساب المواعيد ومنع التعارض والحالات وOTP والوصفات والتقارير والإشعارات | AccountService وDoctorService وScheduleService وAppointmentService وMedicalRecordService وReportService وNotificationService مع EmailSender (SendGrid) وFactory |
| MediCare.Data | القراءة والحفظ وإخفاء EF Core | Entities وApplicationDbContext وRepositories وUnit of Work وMigrations |

| المجال | التقنية | القرار |
| ----: | ----- | ----- |
| الخلفية | C\# وASP.NET Core MVC وIdentity وEF Core | كما في الخطة |
| الأنماط | Repository وUnit of Work وFactory وDI | بدون خدمة فواتير (Billing خارج النطاق) |
| الإيميل | SendGrid | بدل Gmail SMTP؛ يحتاج Sender Verification |
| الواجهة | Razor وBootstrap 5 وFullCalendar.js وChart.js | بدون SignalR |
| قاعدة البيانات | SQL Server محليًا وAzure SQL إنتاجًا | كما في الخطة |
| الاستضافة | Azure App Service | كما في الخطة |
| التكامل والنشر | GitHub Actions | بناء واختبار ونشر عند الدمج في main |
| الاختبار | xUnit وLighthouse | شاملًا اختبار التزامن للحجز المزدوج |
| التصميم | Figma | الرابط: (يُضاف) |
| مكتبة PDF/Excel | تُختار في Sprint 3 | Should، تُقص أولًا عند الضيق |

## **8\. قاعدة البيانات**

الجداول 13 كما كانت، لكن بعد التعديلات: لا Walk-in ولا توقيع، وأُضيف IsDeleted إلى Users. جداول Identity (AspNetUsers وAspNetRoles) تحل محل عمود Role وتُمدَّد بالأعمدة الإضافية.

| الجدول | الأعمدة الأساسية | بعد التعديل |
| ----- | ----- | ----: |
| Users (AspNetUsers) | Id, FullName, Email (فريد), IsEmailVerified, IsSuspended, IsDeleted, CreatedAt | أُضيف IsDeleted؛ الدور من AspNetRoles |
| Patients | Id, UserId (فريد، إلزامي), PatientCode (فريد), PhoneNumber, DateOfBirth, BloodGroup, Allergies, MedicalHistory, IsDeleted | حُذف FullName (الاسم من Users)، وأصبح UserId إلزاميًا |
| Specializations | Id, Name (فريد), IsDeleted | بلا تغيير |
| Doctors | Id, UserId (فريد), SpecializationId, Location, ConsultationFee, RoomNumber, ProfileImage, IsDeleted | بلا تغيير |
| DoctorSchedules | Id, DoctorId, DayOfWeek, StartTime, EndTime, SlotMinutes | بلا تغيير |
| VacationDays | Id, DoctorId, VacationDate, Reason | بلا تغيير |
| Appointments | Id, PatientId, DoctorId, AppointmentDate, StartTime, EndTime, Status, Notes, RowVersion, CreatedAt | بلا تغيير |
| MedicalRecords | Id, AppointmentId (فريد), BloodPressure, HeartRate, Symptoms, Diagnosis, TreatmentNotes, CreatedAt, UpdatedAt | حُذف IsSigned وSignedAt وSignedByDoctorId |
| Prescriptions | Id, MedicalRecordId (فريد), IssuedAt | بلا تغيير |
| PrescriptionItems | Id, PrescriptionId, Medication, Dosage, Frequency | بلا تغيير |
| OtpCodes | Id, UserId, CodeHash, ExpiresAt, FailedAttempts, IsUsed, CreatedAt | بلا تغيير |
| Notifications | Id, UserId, Type, Subject, Status, CreatedAt, SentAt | بلا تغيير |
| AuditLogs | Id, UserId (اختياري), Action, EntityName, EntityId, OccurredAt | الإجراءات: Confirm وReject وCancel فقط |

**القيود الأساسية**

* Appointments.Status: Pending أو Confirmed أو Rejected أو Cancelled أو Completed، والافتراضي Pending.  
* HeartRate بين 20 و300، وConsultationFee صفر أو أكثر، وEndTime بعد StartTime، وDayOfWeek بين 0 و6، وFailedAttempts بين 0 و5.  
* BloodGroup من A+ وA- وB+ وB- وAB+ وAB- وO+ وO-.  
* فريد: Users.Email وPatients.PatientCode وPatients.UserId وDoctors.UserId وSpecializations.Name وVacationDays (DoctorId، VacationDate) وMedicalRecords.AppointmentId وPrescriptions.MedicalRecordId.

**الفهارس**

* فهرس فريد على Appointments (DoctorId، AppointmentDate، StartTime) للمواعيد غير Cancelled وRejected، وهو خط الدفاع الأخير ضد الحجز المزدوج مع RowVersion.  
* Appointments.Status، وAppointments (PatientId، AppointmentDate)، وDoctors.SpecializationId.  
* OtpCodes (UserId، ExpiresAt)، وNotifications (Status، CreatedAt).

يُعاد رسم مخطط ERD ومخطط الكلاسات بعد هذه التعديلات.

## **9\. الخطة الزمنية والمهام**

باقي 12 يومًا لتسليم التنفيذ في 30 نوفمبر، فقُسمت إلى 3 Sprints بدلًا من 4، ويسير الاختبار والتقارير بالتوازي حتى العرض النهائي في 4 ديسمبر. عند الضيق تُقص بنود Should بهذا الترتيب: تصدير PDF/Excel، ثم الوضع الداكن، ثم إيميل التذكير.

![الجدول الزمني · 3 Sprints وتسليمان][image1]

الجدول الزمني · 3 Sprints وتسليمان

| الفترة | المهمة | المسؤول | الدعم |
| ----- | ----: | ----- | ----- |
| Sprint 1 (18–21 نوفمبر) | حل N-Tier وEF Core والجداول وRepositories وUnit of Work | Ahmed | Philopatir |
| Sprint 1 | التسجيل وتسجيل الدخول وIdentity بثلاثة أدوار وOTP | Philopatir | Haneen |
| Sprint 1 | حساب SendGrid وSender Verification وخدمة EmailSender | Ahmed | Philopatir |
| Sprint 1 | قائمة الأطباء والبحث والتصفية | Haneen | Philopatir |
| Sprint 1 | موارد Azure وأول Pipeline في GitHub Actions | Youssef | Ahmed |
| Sprint 1 | تحديث ERD ومخطط الكلاسات والحالات حسب القرارات | Ahmed | Philopatir |
| Sprint 2 (22–26 نوفمبر) | منطق الحجز والإلغاء وإعادة الجدولة ومنع التعارض وقاعدة BR-06 | Philopatir | Youssef |
| Sprint 2 | شاشات FullCalendar ولوحة الدكتور وتأكيد الطلبات ورفضها | Haneen | Philopatir |
| Sprint 2 | إيميلات الحجز والتأكيد والرفض والإلغاء مع Notifications وAuditLogs | Ahmed | Philopatir |
| Sprint 3 (27–30 نوفمبر) | السجلات الطبية والوصفات (المنطق) | Philopatir | Ahmed |
| Sprint 3 | شاشات السجل والوصفة القابلة للطباعة والملف الشخصي | Haneen | Philopatir |
| Sprint 3 | لوحة الأدمن والتقرير اليومي وإدارة المستخدمين والتخصصات | Philopatir | Haneen |
| Sprint 3 | مخططات Chart.js وتصميم متجاوب | Haneen | Dareen |
| Sprint 3 | النشر على Azure وCI/CD وREADME | Youssef | Ahmed |
| بالتوازي (22 نوفمبر – 2 ديسمبر) | خطة الاختبار والاختبارات الآلية، شاملة اختبار الحجز المزدوج | Youssef | الكل |
| 27 نوفمبر – 4 ديسمبر | دليل المستخدم والعرض والإنفوغراف | Dareen | Haneen |
| 27 نوفمبر – 4 ديسمبر | التوثيق الفني (المعمارية، الـ Schema) | Philopatir | Ahmed |

## **10\. المخاطر ومؤشرات الأداء**

هناك 12 خطرًا: 6 High و5 Medium و1 Low. حُذف R14 (تنبيهات SignalR) واستُبدل R1 و R2 القديمان بخطر ضغط الوقت. الدرجة \= الاحتمال × الأثر.

| الرقم | الخطر | الدرجة | التخفيف | المسؤول |
| ----- | ----: | ----- | ----- | ----- |
| R1 | ضغط الوقت: 12 يومًا للتنفيذ | High (9) | 3 Sprints، ونشر على Azure في أول Sprint، وقص بنود Should بالترتيب المتفق عليه | Dareen |
| R3 | حجز مزدوج | High (6) | فهرس فريد للطبيب والموعد، وفحص وحفظ في Unit of Work واحدة، واختبارات تزامن | Philopatir |
| R4 | عدم وصول OTP أو الإيميل | High (6) | Sender Verification في SendGrid، والمفتاح خارج الكود، وإعادة إرسال بفاصل، وتسجيل الفشل | Ahmed |
| R5 | تخمين OTP أو إعادة استخدامه | High (6) | مُشفَّر، صالح 10 دقائق، قفل بعد 5 محاولات | Philopatir |
| R6 | كشف بيانات طبية | High (6) | صلاحيات على كل صفحة، كل مريض يرى سجله فقط، HTTPS، بيانات تجريبية | Philopatir |
| R10 | مشاكل Azure | High (6) | التأكد من الوصول في Sprint 1 وأقل فئة مناسبة وREADME للتشغيل المحلي كبديل | Youssef |
| R7 | حسابات أطباء وهمية | Medium (4) | تظهر بعد تأكيد الإيميل واكتمال الملف، وللأدمن الإيقاف أو الحذف | Philopatir |
| R8 | غياب عضو | Medium (4) | لكل مهمة مسؤول وداعم، ومراجعة Pull Requests | Dareen |
| R9 | تعارض الدمج | Medium (4) | فروع ميزات وmain محمي ودمج يومي صغير | Youssef |
| R11 | مواعيد متاحة خاطئة | Medium (4) | اختبارات للتوفر والإجازات والجدولة وقاعدة BR-06 | Philopatir |
| R13 | توسع النطاق | Medium (4) | هذا المستند هو الحد؛ الأفكار الجديدة للتحسينات المستقبلية | Dareen |
| R12 | بطء اللوحة | Low (2) | فهارس وتقسيم الصفحات وحساب الإجماليات في قاعدة البيانات | Ahmed |

مؤشرات الأداء الـ 12 كما في الخطة، بلا تغيير في الأهداف:

| الرقم | المؤشر | الهدف |
| ----- | ----: | ----- |
| KPI-01 | زمن استجابة الصفحات | 95% أقل من ثانيتين |
| KPI-02 | سرعة الحجز | أقل من دقيقتين من البحث إلى الحجز |
| KPI-03 | وصول OTP | 95% خلال دقيقة |
| KPI-04 | تحميل لوحة الأدمن | أقل من 5 ثوانٍ مع 10,000 موعد |
| KPI-05 | توفر النظام | 99% خلال فترة التقييم |
| KPI-06 | الحجز المزدوج | صفر مواعيد نشطة مكررة |
| KPI-07 | الصلاحيات والأسرار | نجاح 100% من اختبارات الصلاحيات وصفر أسرار في المستودع |
| KPI-08 | تأكيد التسجيل | 90% من التسجيلات تُؤكد بـ OTP |
| KPI-09 | إتمام المهام | 80% من المستخدمين يحجزون أو يؤكدون بلا مساعدة |
| KPI-10 | نجاح الاختبارات | 90% ولا خطأ حرج مفتوح |
| KPI-11 | الوصول | Lighthouse 90 فأكثر |
| KPI-12 | مواعيد DEPI | 4 من 4 و90% من المهام |

## **11\. بنود مفتوحة**

هذه نقاط لم تُحسم بعد، وأغلبها تحديثات للمخططات القديمة بعد القرارات.

* ☐ إضافة رابط Figma في القسم 7\.  
* ☐ إعادة رسم ERD ومخطط الكلاسات ومخطط حالة السجل (بلا توقيع أو Addenda أو Walk-in).  
* ☐ تحديث مخططات المكونات والنشر والاستخدام لـ SendGrid وحذف Walk-in، وحذف كلمة Billing من مخطط الطبقات.  
* ☐ تحديث مخطط تسلسل الحجز ليُظهر Unit of Work والإيميل.  
* ☐ تحديد هل يُعاد استخدام بريد مستخدم محذوف (حاليًا يبقى مستخدمًا لأن Email فريد).  
* ☐ تحديد حالة موعد لم يحضره المريض (لا توجد حالة No-show، فيبقى Confirmed).  
* ☐ التأكد من حدود خطة SendGrid المتاحة لحساب الفريق قبل الاختبار.  
* ☐ تأكيد ترتيب القص عند الضيق (افتُرض ترتيب R2 القديم لأنه لم يُحدد تفضيل).

[image1]: <data:image/png;base64,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>