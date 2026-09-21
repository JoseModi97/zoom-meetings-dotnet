Total operations: 186
Typed in v1: 38
Raw-only (still fully callable via CallAsync): 148

| Domain | Total | Typed v1 | Raw-only |
|---|---|---|---|
| Devices/H.323 | 17 | 0 | 17 |
| Live Meeting Controls | 4 | 0 | 4 |
| Meeting Summaries | 4 | 3 | 1 |
| Meetings core | 27 | 8 | 19 |
| Polls | 7 | 5 | 2 |
| Recordings/Archiving | 23 | 5 | 18 |
| Registrants | 8 | 5 | 3 |
| Reports | 24 | 2 | 22 |
| SIP Phones | 4 | 0 | 4 |
| TSP | 8 | 0 | 8 |
| Templates | 2 | 0 | 2 |
| Tracking Fields | 5 | 0 | 5 |
| Webinars | 53 | 10 | 43 |


### Devices/H.323
| Method | Path | operationId | v1 status |
|---|---|---|---|
| GET | `/devices` | listDevices | Raw |
| POST | `/devices` | addDevice | Raw |
| GET | `/devices/groups` | Getzdmgroupinfo | Raw |
| POST | `/devices/zpa/assignment` | Assigndevicetoauser/commonarea | Raw |
| GET | `/devices/zpa/settings` | GetZpaDeviceListProfileSettingOfaUser | Raw |
| POST | `/devices/zpa/upgrade` | UpgradeZpas/app | Raw |
| DELETE | `/devices/zpa/vendors/{vendor}/mac_addresses/{macAddress}` | DeleteZpaDeviceByVendorAndMacAddress | Raw |
| GET | `/devices/zpa/zdm_groups/{zdmGroupId}/versions` | GetZpaVersioninfo | Raw |
| DELETE | `/devices/{deviceId}` | deleteDevice | Raw |
| GET | `/devices/{deviceId}` | getDevice | Raw |
| PATCH | `/devices/{deviceId}` | updateDevice | Raw |
| PATCH | `/devices/{deviceId}/assign_group` | assginGroup | Raw |
| PATCH | `/devices/{deviceId}/assignment` | changeDeviceAssociation | Raw |
| GET | `/h323/devices` | deviceList | Raw |
| POST | `/h323/devices` | deviceCreate | Raw |
| DELETE | `/h323/devices/{deviceId}` | deviceDelete | Raw |
| PATCH | `/h323/devices/{deviceId}` | deviceUpdate | Raw |

### Live Meeting Controls
| Method | Path | operationId | v1 status |
|---|---|---|---|
| DELETE | `/live_meetings/{meetingId}/chat/messages/{messageId}` | deleteMeetingChatMessageById | Raw |
| PATCH | `/live_meetings/{meetingId}/chat/messages/{messageId}` | updateMeetingChatMessageById | Raw |
| PATCH | `/live_meetings/{meetingId}/events` | inMeetingControl | Raw |
| PATCH | `/live_meetings/{meetingId}/rtms_app/status` | meetingRTMSStatusUpdate | Raw |

### Meeting Summaries
| Method | Path | operationId | v1 status |
|---|---|---|---|
| GET | `/meetings/meeting_summaries` | Listmeetingsummaries | Typed |
| DELETE | `/meetings/{meetingId}/meeting_summary` | Deletemeetingorwebinarsummary | Typed |
| GET | `/meetings/{meetingId}/meeting_summary` | Getameetingsummary | Typed |
| GET | `/users/{userId}/meeting_summaries` | ListUserMeetingSummaries | Raw |

### Meetings core
| Method | Path | operationId | v1 status |
|---|---|---|---|
| DELETE | `/meetings/{meetingId}` | meetingDelete | Typed |
| GET | `/meetings/{meetingId}` | meeting | Typed |
| PATCH | `/meetings/{meetingId}` | meetingUpdate | Typed |
| GET | `/meetings/{meetingId}/invitation` | meetingInvitation | Typed |
| POST | `/meetings/{meetingId}/invite_links` | meetingInviteLinksCreate | Raw |
| GET | `/meetings/{meetingId}/jointoken/live_streaming` | meetingLiveStreamingJoinToken | Raw |
| GET | `/meetings/{meetingId}/jointoken/local_archiving` | meetingLocalArchivingArchiveToken | Raw |
| GET | `/meetings/{meetingId}/jointoken/local_recording` | meetingLocalRecordingJoinToken | Raw |
| GET | `/meetings/{meetingId}/livestream` | getMeetingLiveStreamDetails | Raw |
| PATCH | `/meetings/{meetingId}/livestream` | meetingLiveStreamUpdate | Raw |
| PATCH | `/meetings/{meetingId}/livestream/status` | meetingLiveStreamStatusUpdate | Raw |
| DELETE | `/meetings/{meetingId}/open_apps` | meetingAppDelete | Raw |
| POST | `/meetings/{meetingId}/open_apps` | meetingAppAdd | Raw |
| POST | `/meetings/{meetingId}/sip_dialing` | getSipDialingWithPasscode | Raw |
| PUT | `/meetings/{meetingId}/status` | meetingStatus | Typed |
| DELETE | `/meetings/{meetingId}/survey` | meetingSurveyDelete | Raw |
| GET | `/meetings/{meetingId}/survey` | meetingSurveyGet | Raw |
| PATCH | `/meetings/{meetingId}/survey` | meetingSurveyUpdate | Raw |
| GET | `/meetings/{meetingId}/token` | meetingToken | Raw |
| GET | `/past_meetings/{meetingId}` | pastMeetingDetails | Raw |
| GET | `/past_meetings/{meetingId}/instances` | pastMeetings | Raw |
| GET | `/past_meetings/{meetingId}/participants` | pastMeetingParticipants | Raw |
| GET | `/past_meetings/{meetingId}/qa` | listPastMeetingQA | Raw |
| GET | `/users/{userId}/meetings` | meetings | Typed |
| POST | `/users/{userId}/meetings` | meetingCreate | Typed |
| GET | `/users/{userId}/pac` | userPACs | Raw |
| GET | `/users/{userId}/upcoming_meetings` | listUpcomingMeeting | Typed |

### Polls
| Method | Path | operationId | v1 status |
|---|---|---|---|
| POST | `/meetings/{meetingId}/batch_polls` | createBatchPolls | Raw |
| GET | `/meetings/{meetingId}/polls` | meetingPolls | Typed |
| POST | `/meetings/{meetingId}/polls` | meetingPollCreate | Typed |
| DELETE | `/meetings/{meetingId}/polls/{pollId}` | meetingPollDelete | Typed |
| GET | `/meetings/{meetingId}/polls/{pollId}` | meetingPollGet | Typed |
| PUT | `/meetings/{meetingId}/polls/{pollId}` | meetingPollUpdate | Typed |
| GET | `/past_meetings/{meetingId}/polls` | listPastMeetingPolls | Raw |

### Recordings/Archiving
| Method | Path | operationId | v1 status |
|---|---|---|---|
| GET | `/archive_files` | listArchivedFiles | Raw |
| GET | `/archive_files/download_audit` | listArchiveFileDownloadAudit | Raw |
| GET | `/archive_files/statistics` | getArchivedFileStatistics | Raw |
| PATCH | `/archive_files/{fileId}` | updateArchivedFile | Raw |
| DELETE | `/meetings/{meetingId}/recordings` | recordingDelete | Typed |
| GET | `/meetings/{meetingId}/recordings` | recordingGet | Typed |
| GET | `/meetings/{meetingId}/recordings/analytics_details` | analytics_details | Raw |
| GET | `/meetings/{meetingId}/recordings/analytics_summary` | analytics_summary | Raw |
| GET | `/meetings/{meetingId}/recordings/registrants` | meetingRecordingRegistrants | Raw |
| POST | `/meetings/{meetingId}/recordings/registrants` | meetingRecordingRegistrantCreate | Raw |
| GET | `/meetings/{meetingId}/recordings/registrants/questions` | recordingRegistrantsQuestionsGet | Raw |
| PATCH | `/meetings/{meetingId}/recordings/registrants/questions` | recordingRegistrantQuestionUpdate | Raw |
| PUT | `/meetings/{meetingId}/recordings/registrants/status` | meetingRecordingRegistrantStatus | Raw |
| GET | `/meetings/{meetingId}/recordings/settings` | recordingSettingUpdate | Typed |
| PATCH | `/meetings/{meetingId}/recordings/settings` | recordingSettingsUpdate | Typed |
| DELETE | `/meetings/{meetingId}/recordings/{recordingId}` | recordingDeleteOne | Typed |
| PUT | `/meetings/{meetingId}/recordings/{recordingId}/status` | recordingStatusUpdateOne | Raw |
| DELETE | `/meetings/{meetingId}/transcript` | DeleteMeetingTranscript | Raw |
| GET | `/meetings/{meetingId}/transcript` | GetMeetingTranscript | Raw |
| PUT | `/meetings/{meetingUUID}/recordings/status` | recordingStatusUpdate | Raw |
| DELETE | `/past_meetings/{meetingUUID}/archive_files` | deleteArchivedFiles | Raw |
| GET | `/past_meetings/{meetingUUID}/archive_files` | getArchivedFiles | Raw |
| GET | `/users/{userId}/recordings` | recordingsList | Raw |

### Registrants
| Method | Path | operationId | v1 status |
|---|---|---|---|
| POST | `/meetings/{meetingId}/batch_registrants` | addBatchRegistrants | Raw |
| GET | `/meetings/{meetingId}/registrants` | meetingRegistrants | Typed |
| POST | `/meetings/{meetingId}/registrants` | meetingRegistrantCreate | Typed |
| GET | `/meetings/{meetingId}/registrants/questions` | meetingRegistrantsQuestionsGet | Raw |
| PATCH | `/meetings/{meetingId}/registrants/questions` | meetingRegistrantQuestionUpdate | Raw |
| PUT | `/meetings/{meetingId}/registrants/status` | meetingRegistrantStatus | Typed |
| DELETE | `/meetings/{meetingId}/registrants/{registrantId}` | meetingregistrantdelete | Typed |
| GET | `/meetings/{meetingId}/registrants/{registrantId}` | meetingRegistrantGet | Typed |

### Reports
| Method | Path | operationId | v1 status |
|---|---|---|---|
| GET | `/report/activities` | reportSignInSignOutActivities | Raw |
| GET | `/report/billing` | getBillingReport | Raw |
| GET | `/report/billing/invoices` | getBillingInvoicesReports | Raw |
| GET | `/report/cloud_recording` | reportCloudRecording | Raw |
| GET | `/report/daily` | reportDaily | Raw |
| GET | `/report/disclaimer` | Getdisclaimerreport | Raw |
| GET | `/report/history_meetings` | Gethistorymeetingandwebinarlist | Raw |
| GET | `/report/meeting_activities` | reportMeetingactivitylogs | Raw |
| GET | `/report/meetings/{meetingId}` | reportMeetingDetails | Typed |
| GET | `/report/meetings/{meetingId}/participants` | reportMeetingParticipants | Typed |
| GET | `/report/meetings/{meetingId}/polls` | reportMeetingPolls | Raw |
| GET | `/report/meetings/{meetingId}/qa` | reportMeetingQA | Raw |
| GET | `/report/meetings/{meetingId}/survey` | reportMeetingSurvey | Raw |
| GET | `/report/operationlogs` | reportOperationLogs | Raw |
| GET | `/report/remote_support` | Getremotesupportreport | Raw |
| GET | `/report/telephone` | reportTelephone | Raw |
| GET | `/report/upcoming_events` | reportUpcomingEvents | Raw |
| GET | `/report/users` | reportUsers | Raw |
| GET | `/report/users/{userId}/meetings` | reportMeetings | Raw |
| GET | `/report/webinars/{webinarId}` | reportWebinarDetails | Raw |
| GET | `/report/webinars/{webinarId}/participants` | reportWebinarParticipants | Raw |
| GET | `/report/webinars/{webinarId}/polls` | reportWebinarPolls | Raw |
| GET | `/report/webinars/{webinarId}/qa` | reportWebinarQA | Raw |
| GET | `/report/webinars/{webinarId}/survey` | reportWebinarSurvey | Raw |

### SIP Phones
| Method | Path | operationId | v1 status |
|---|---|---|---|
| GET | `/sip_phones/phones` | ListSIPPhonePhones | Raw |
| POST | `/sip_phones/phones` | EnableSIPPhonePhones | Raw |
| DELETE | `/sip_phones/phones/{phoneId}` | deleteSIPPhonePhones | Raw |
| PATCH | `/sip_phones/phones/{phoneId}` | UpdateSIPPhonePhones | Raw |

### TSP
| Method | Path | operationId | v1 status |
|---|---|---|---|
| GET | `/tsp` | tsp | Raw |
| PATCH | `/tsp` | tspUpdate | Raw |
| GET | `/users/{userId}/tsp` | userTSPs | Raw |
| POST | `/users/{userId}/tsp` | userTSPCreate | Raw |
| PATCH | `/users/{userId}/tsp/settings` | tspUrlUpdate | Raw |
| DELETE | `/users/{userId}/tsp/{tspId}` | userTSPDelete | Raw |
| GET | `/users/{userId}/tsp/{tspId}` | userTSP | Raw |
| PATCH | `/users/{userId}/tsp/{tspId}` | userTSPUpdate | Raw |

### Templates
| Method | Path | operationId | v1 status |
|---|---|---|---|
| GET | `/users/{userId}/meeting_templates` | listMeetingTemplates | Raw |
| POST | `/users/{userId}/meeting_templates` | meetingTemplateCreate | Raw |

### Tracking Fields
| Method | Path | operationId | v1 status |
|---|---|---|---|
| GET | `/tracking_fields` | trackingfieldList | Raw |
| POST | `/tracking_fields` | trackingfieldCreate | Raw |
| DELETE | `/tracking_fields/{fieldId}` | trackingfieldDelete | Raw |
| GET | `/tracking_fields/{fieldId}` | trackingfieldGet | Raw |
| PATCH | `/tracking_fields/{fieldId}` | trackingfieldUpdate | Raw |

### Webinars
| Method | Path | operationId | v1 status |
|---|---|---|---|
| DELETE | `/live_webinars/{webinarId}/chat/messages/{messageId}` | deleteWebinarChatMessageById | Raw |
| GET | `/past_webinars/{webinarId}/absentees` | webinarAbsentees | Raw |
| GET | `/past_webinars/{webinarId}/instances` | pastWebinars | Raw |
| GET | `/past_webinars/{webinarId}/participants` | listWebinarParticipants | Raw |
| GET | `/past_webinars/{webinarId}/polls` | listPastWebinarPollResults | Raw |
| GET | `/past_webinars/{webinarId}/qa` | listPastWebinarQA | Raw |
| GET | `/users/{userId}/webinar_templates` | listWebinarTemplates | Raw |
| POST | `/users/{userId}/webinar_templates` | webinarTemplateCreate | Raw |
| GET | `/users/{userId}/webinars` | webinars | Typed |
| POST | `/users/{userId}/webinars` | webinarCreate | Typed |
| DELETE | `/webinars/{webinarId}` | webinarDelete | Typed |
| GET | `/webinars/{webinarId}` | webinar | Typed |
| PATCH | `/webinars/{webinarId}` | webinarUpdate | Typed |
| POST | `/webinars/{webinarId}/batch_registrants` | addBatchWebinarRegistrants | Raw |
| GET | `/webinars/{webinarId}/branding` | getWebinarBranding | Raw |
| DELETE | `/webinars/{webinarId}/branding/name_tags` | deleteWebinarBrandingNameTag | Raw |
| POST | `/webinars/{webinarId}/branding/name_tags` | createWebinarBrandingNameTag | Raw |
| PATCH | `/webinars/{webinarId}/branding/name_tags/{nameTagId}` | updateWebinarBrandingNameTag | Raw |
| DELETE | `/webinars/{webinarId}/branding/virtual_backgrounds` | deleteWebinarBrandingVB | Raw |
| PATCH | `/webinars/{webinarId}/branding/virtual_backgrounds` | setWebinarBrandingVB | Raw |
| POST | `/webinars/{webinarId}/branding/virtual_backgrounds` | uploadWebinarBrandingVB | Raw |
| DELETE | `/webinars/{webinarId}/branding/wallpaper` | deleteWebinarBrandingWallpaper | Raw |
| POST | `/webinars/{webinarId}/branding/wallpaper` | uploadWebinarBrandingWallpaper | Raw |
| POST | `/webinars/{webinarId}/invite_links` | webinarInviteLinksCreate | Raw |
| GET | `/webinars/{webinarId}/jointoken/live_streaming` | webinarLiveStreamingJoinToken | Raw |
| GET | `/webinars/{webinarId}/jointoken/local_archiving` | webinarLocalArchivingArchiveToken | Raw |
| GET | `/webinars/{webinarId}/jointoken/local_recording` | webinarLocalRecordingJoinToken | Raw |
| GET | `/webinars/{webinarId}/livestream` | getWebinarLiveStreamDetails | Raw |
| PATCH | `/webinars/{webinarId}/livestream` | webinarLiveStreamUpdate | Raw |
| PATCH | `/webinars/{webinarId}/livestream/status` | webinarLiveStreamStatusUpdate | Raw |
| DELETE | `/webinars/{webinarId}/panelists` | webinarPanelistsDelete | Raw |
| GET | `/webinars/{webinarId}/panelists` | webinarPanelists | Raw |
| POST | `/webinars/{webinarId}/panelists` | webinarPanelistCreate | Raw |
| DELETE | `/webinars/{webinarId}/panelists/{panelistId}` | webinarPanelistDelete | Raw |
| GET | `/webinars/{webinarId}/polls` | webinarPolls | Raw |
| POST | `/webinars/{webinarId}/polls` | webinarPollCreate | Raw |
| DELETE | `/webinars/{webinarId}/polls/{pollId}` | webinarPollDelete | Raw |
| GET | `/webinars/{webinarId}/polls/{pollId}` | webinarPollGet | Raw |
| PUT | `/webinars/{webinarId}/polls/{pollId}` | webinarPollUpdate | Raw |
| GET | `/webinars/{webinarId}/registrants` | webinarRegistrants | Typed |
| POST | `/webinars/{webinarId}/registrants` | webinarRegistrantCreate | Typed |
| GET | `/webinars/{webinarId}/registrants/questions` | webinarRegistrantsQuestionsGet | Raw |
| PATCH | `/webinars/{webinarId}/registrants/questions` | webinarRegistrantQuestionUpdate | Raw |
| PUT | `/webinars/{webinarId}/registrants/status` | webinarRegistrantStatus | Typed |
| DELETE | `/webinars/{webinarId}/registrants/{registrantId}` | deleteWebinarRegistrant | Typed |
| GET | `/webinars/{webinarId}/registrants/{registrantId}` | webinarRegistrantGet | Typed |
| POST | `/webinars/{webinarId}/sip_dialing` | getWebinarSipDialingWithPasscode | Raw |
| PUT | `/webinars/{webinarId}/status` | webinarStatus | Raw |
| DELETE | `/webinars/{webinarId}/survey` | webinarSurveyDelete | Raw |
| GET | `/webinars/{webinarId}/survey` | webinarSurveyGet | Raw |
| PATCH | `/webinars/{webinarId}/survey` | webinarSurveyUpdate | Raw |
| GET | `/webinars/{webinarId}/token` | webinarToken | Raw |
| GET | `/webinars/{webinarId}/tracking_sources` | getTrackingSources | Raw |