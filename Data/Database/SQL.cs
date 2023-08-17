namespace TheEstate.Data.Database
{
    public class SQL
    {
        #region App
        public const string TestFetch = "SELECT estate_name FROM estate WHERE estate_id = #";
        public const string TemplateByIdQry = "SELECT template_id, template_desc, sms_content, email_content, template_url, email_subject FROM template where template_id = #";
        public const string SettingByGroupQry = "SELECT setting_id, setting_value, setting_group, setting_desc FROM settings where setting_group = #";
        public const string SettingByIdQry = "SELECT setting_id, setting_value, setting_group, setting_desc FROM settings where setting_id = #";
        public const string SettingValueByIdQry = "SELECT setting_value FROM settings where setting_id = #";
        #endregion


        #region Auth
        public const string CheckExistsUsernameQry = "select count(1) from userprofile where username = #";
        public const string CreateUserQry = @"INSERT INTO 
                            userprofile (profile_id, username, password, login_method, date_created, status, otp, otp_timestamp, onboarding_stage)
                            VALUES(#, #, #, #, #, #, #, #, #)";
        public const string AuthUserQry = "select u.profile_id, u.username, u.password, u.login_method, u.emailaddress, u.mobileno, u.firstname, u.lastname, u.date_created, u.status, u.image_url, u.onboarding_stage from userprofile u where username = #";
        public const string AuthUserQry1 = @"select 
                            u.profile_id, u.username, u.password, u.login_method, u.emailaddress, u.mobileno, u.firstname, u.lastname, u.date_created, u.status,
                            r.estate_id, r.household_id, r.resident_id, r.resident_category, r.resident_code, r.resident_firstname, r.resident_lastname, r.resident_mobileno, r.resident_emailaddress,
                            e.estate_name, e.estate_desc  
                            from userprofile u 
                            left join mc_resident r on r.profile_id = u.profile_id 
                            left join mc_estate e on e.estate_id = r.estate_id
                            where u.username = # and r.is_default = 'Y'";
        public const string OtpByProfileIdQry = "select otp, otp_timestamp from userprofile u where profile_id = #";
        public const string VerifyOtp = "update userprofile set status = #, otp = #, onboarding_stage = # where profile_id = #";
        public const string ResetOtpQry = "update userprofile set otp = #, otp_timestamp = # where profile_id = #";
        #endregion


        #region Profile
        public const string ProfileByIdQry = @"SELECT 
                            profile_id, username, login_method, emailaddress, mobileno, firstname, lastname, date_created, last_passwords, last_password_change, login_attempts, status, image_url, onboarding_stage
                            FROM userprofile
                            WHERE profile_id = #";
        public const string ProfileByUsernameQry = @"SELECT 
                            profile_id, username, login_method, emailaddress, mobileno, firstname, lastname, date_created, last_passwords, last_password_change, login_attempts, status, image_url, onboarding_stage
                            FROM userprofile
                            WHERE username = #";
        public const string ProfileSetupQry = @"UPDATE userprofile SET  emailaddress=#, mobileno=#, firstname=#, lastname=#, image_url=#, image_name=#, image_type=#, image_size=#, onboarding_stage=# WHERE profile_id=#";
        public const string AddProfileToResidentQry = "update mc_resident set status = #, profile_id = #, date_activated = #, is_default = # where resident_code = #";
        public const string ResidentOnboardingOptQry = "update mc_resident set profile_id = #, otp=#, otp_timestamp=# where resident_code = #";
        public const string ResidentOnboardVerifyOtpQry = "update mc_resident set status = #, date_activated = #, otp = # where resident_code = #";
        public const string ResidnetOtpByResidenCodeQry = "select otp, otp_timestamp from mc_resident u where resident_code = #";
        #endregion


        #region Resident
        public const string CheckExistsResidentCode = "select count(1) from mc_resident where resident_code = #";
        public const string ResidencyQry = @"select 
                            mr.estate_id, mr.household_id, mr.resident_id, mr.resident_category, mr.resident_code, mr.resident_firstname, mr.resident_lastname, mr.resident_mobileno, mr.resident_emailaddress, mr.status, mr.profile_id, mr.date_activated,
                            me.estate_name, me.estate_desc, me.image_url,
                            mh.household_label, mh.property_id,
                            mp.property_no, mp.property_type, mp.property_category,
                            ms.street_id, ms.street_name,
                            u.image_url profilephoto
                            from mc_resident mr
                            join mc_estate me on me.estate_id = mr.estate_id 
                            join mc_household mh on mh.household_id = mr.household_id 
                            left join mc_property mp on mp.property_id = mh.property_id 
                            left join mc_zipcode mz on mz.zipcode_id = mp.zipcode_id 
                            left join mc_street ms on ms.street_id = mz.street_id
                            left join userprofile u on u.profile_id = mr.profile_id
                            where mr.estate_id like # and mr.household_id like # and mr.resident_id like # and mr.resident_code like #
                            limit # offset #";
        public const string ResidencyQryQryCount = @"select 
                            count(1)
                            from mc_resident mr
                            join mc_estate me on me.estate_id = mr.estate_id 
                            join mc_household mh on mh.household_id = mr.household_id 
                            left join mc_property mp on mp.property_id = mh.property_id 
                            left join mc_zipcode mz on mz.zipcode_id = mp.zipcode_id 
                            left join mc_street ms on ms.street_id = mz.street_id
                            where mr.estate_id like # and mr.household_id like # and mr.resident_id like # and mr.resident_code like #";
        public const string ResidentByProfileId = @"select 
                            mr.estate_id, mr.household_id, mr.resident_id, mr.resident_category, mr.resident_code, mr.resident_firstname, mr.resident_lastname, mr.resident_mobileno, mr.resident_emailaddress, mr.status, mr.profile_id, mr.date_activated,
                            me.estate_name, me.estate_desc, me.image_url,
                            mh.household_label, mh.property_id,
                            mp.property_no, mp.property_type, mp.property_category,
                            ms.street_id, ms.street_name,
                            mz.zone_id
                            from mc_resident mr
                            join mc_estate me on me.estate_id = mr.estate_id 
                            join mc_household mh on mh.household_id = mr.household_id 
                            left join mc_property mp on mp.property_id = mh.property_id 
                            left join mc_zipcode mz on mz.zipcode_id = mp.zipcode_id 
                            left join mc_street ms on ms.street_id = mz.street_id
                            where mr.profile_id = #";
        public const string ResidentByResidentCodeQry = @"select 
                            mr.estate_id, mr.household_id, mr.resident_id, mr.resident_category, mr.resident_code, mr.resident_firstname, mr.resident_lastname, mr.resident_mobileno, mr.resident_emailaddress, mr.status, mr.profile_id, mr.date_activated,
                            me.estate_name, me.estate_desc, me.image_url,
                            mh.household_label, mh.property_id,
                            mp.property_no, mp.property_type, mp.property_category,
                            ms.street_id, ms.street_name,
                            u.emailaddress profile_email, u.mobileno profile_mobile, u.login_method 
                            from mc_resident mr
                            left join mc_estate me on me.estate_id = mr.estate_id 
                            left join mc_household mh on mh.household_id = mr.household_id 
                            left join mc_property mp on mp.property_id = mh.property_id 
                            left join mc_zipcode mz on mz.zipcode_id = mp.zipcode_id 
                            left join mc_street ms on ms.street_id = mz.street_id 
                            left join userprofile u on u.profile_id = mr.profile_id or u.mobileno = mr.resident_mobileno or u.emailaddress = mr.resident_emailaddress  
                            where mr.resident_code = #";
        public const string ResidentByHouseholdId = @"select 
                            mr.estate_id, mr.household_id, mr.resident_id, mr.resident_category, mr.resident_code, mr.resident_firstname, mr.resident_lastname, mr.resident_mobileno, mr.resident_emailaddress, mr.status, mr.profile_id, mr.date_activated,
                            me.estate_name, me.estate_desc, me.image_url,
                            mh.household_label, mh.property_id,
                            mp.property_no, mp.property_type, mp.property_category,
                            ms.street_id, ms.street_name
                            from mc_resident mr
                            join mc_estate me on me.estate_id = mr.estate_id 
                            join mc_household mh on mh.household_id = mr.household_id 
                            left join mc_property mp on mp.property_id = mh.property_id 
                            left join mc_zipcode mz on mz.zipcode_id = mp.zipcode_id 
                            left join mc_street ms on ms.street_id = mz.street_id
                            where mr.household_id = #";
        public const string ResidentCreateQry = @"INSERT INTO mc_resident
                            (estate_id, household_id, resident_id, resident_category, resident_code, resident_firstname, resident_lastname, resident_mobileno, resident_emailaddress, status, profile_id, date_created, created_by, otp, otp_timestamp)
                            VALUES(#, #, #, #, #, #, #, #, #, #, #, #, #, #, #)";
        #endregion


        #region Estate
        public const string EstatesQry = @"select 
                            me.estate_id, me.estate_name, me.estate_desc, me.longitute, me.latitude, me.status, me.estate_code, me.image_url, me.entry_by, me.entry_date, me.last_modified_by, me.last_modified_date, 
                            (select count(1) from mc_zone mz where mz.estate_id = me.estate_id) no_of_zone,
                            (select count(1) from mc_street ms where ms.estate_id = me.estate_id) no_of_street
                            from mc_estate me
                            where upper(me.estate_name) like # and upper(me.status) like # and upper(me.estate_code) like # and is_deleted = 0
                            order by me.estate_name 
                            limit # offset #";
        public const string EstateByIdQry = @"select 
                            me.estate_id, me.estate_name, me.estate_desc, me.longitute, me.latitude, me.status, me.estate_code, me.image_url, me.entry_by, me.entry_date , me.last_modified_by, me.last_modified_date, 
                            (select count(1) from mc_zone mz where mz.estate_id = me.estate_id) no_of_zone,
                            (select count(1) from mc_street ms where ms.estate_id = me.estate_id) no_of_street
                            from mc_estate me
                            where me.estate_id = # and is_deleted = 0";
        public const string EstatesCountQry = @"select count(1) 
                            from mc_estate me 
                            where upper(me.estate_name) like # and upper(me.status) like # and upper(me.estate_code) like # and is_deleted = 0";
        public const string EstateCreateQry = @"INSERT INTO mc_estate
                            (estate_id, estate_name, estate_desc, longitute, latitude, status, estate_code, image_url, entry_by, entry_date)
                            VALUES(#, #, #, #, #, #, #, #, #, #)";
        public const string EstateUpdateQry = @"UPDATE mc_estate
                            SET estate_name=#, estate_desc=#, longitute=#, latitude=#, status=#, estate_code=#, image_url=#, last_modified_by=#, last_modified_date=#
                            WHERE estate_id=#";
        public const string EstateDeactivateQry = "update mc_estate set is_deleted=1 where estate_id=#";
        #endregion


        #region Property
        public const string PropertiesQry = @"select 
                            mp.estate_id, mp.property_id, mp.zipcode_id, mp.property_no, mp.property_type, mp.property_category, mp.status, mp.remarks, mp.owners_firstname, mp.owners_lastname, mp.owners_mobileno, mp.owners_email, mp.owners_gender, mp.owners_other_address, mp.entry_by, mp.entry_date, mp.last_modified_by, mp.last_modified_date, mp.property_image, 
                            mzc.zipcode_id, 
                            mz.zone_id, mz.zone_code, mz.zone_name, mz.zone_type,
                            ms.street_id, ms.street_code, ms.street_name, ms.street_type,
                            me.estate_name, me.estate_desc
                            from mc_property mp
                            join mc_zipcode mzc on mzc.zipcode_id = mp.zipcode_id
                            join mc_zone mz on mz.zone_id = mzc.zone_id  
                            join mc_street ms on ms.street_id = mzc.street_id
                            join mc_estate me on me.estate_id = mp.estate_id 
                            where mp.estate_id like # and mp.property_type like # and mp.property_category like # and mp.is_deleted = 0 and mz.zone_id like # and ms.street_id like #
                            order by mp.property_no 
                            limit # offset #";
        public const string PropertiesQryCount = @"select count(1)
                            from mc_property mp
                            join mc_zipcode mzc on mzc.zipcode_id = mp.zipcode_id
                            join mc_zone mz on mz.zone_id = mzc.zone_id  
                            join mc_street ms on ms.street_id = mzc.street_id
                            join mc_estate me on me.estate_id = mp.estate_id 
                            where mp.estate_id like # and mp.property_type like # and mp.property_category like # and mp.is_deleted = 0 and mz.zone_id like # and ms.street_id like #";
        public const string PropertyByIdQry = @"select 
                            mp.estate_id, mp.property_id, mp.zipcode_id, mp.property_no, mp.property_type, mp.property_category, mp.status, mp.remarks, mp.owners_firstname, mp.owners_lastname, mp.owners_mobileno, mp.owners_email, mp.owners_gender, mp.owners_other_address, mp.entry_by, mp.entry_date, mp.last_modified_by, mp.last_modified_date, mp.property_image, 
                            mzc.zipcode_id, 
                            mz.zone_id, mz.zone_code, mz.zone_name, mz.zone_type,
                            ms.street_id, ms.street_code, ms.street_name, ms.street_type,
                            me.estate_name, me.estate_desc
                            from mc_property mp
                            join mc_zipcode mzc on mzc.zipcode_id = mp.zipcode_id
                            join mc_zone mz on mz.zone_id = mzc.zone_id  
                            join mc_street ms on ms.street_id = mzc.street_id
                            join mc_estate me on me.estate_id = mp.estate_id 
                            where mp.property_id = # and mp.is_deleted = 0";
        public const string PropertyCreateQry = @"INSERT INTO 
                            mc_property (estate_id, property_id, zipcode_id, property_no, property_type, property_category, status, remarks, owners_firstname, owners_lastname, owners_mobileno, owners_email, owners_gender, owners_other_address, entry_by, entry_date, property_image)
                            VALUES(#, #, #, #, #, #, #, #, #, #, #, #, #, #, #, #, #)";
        public const string PropertyUpdateQry = @"UPDATE mc_property
                            SET estate_id=#, zipcode_id=#, property_no=#, property_type=#, property_category=#, status=#, remarks=#, owners_firstname=#, owners_lastname=#, owners_mobileno=#, owners_email=#, owners_gender=#, owners_other_address=#, last_modified_by=#, last_modified_date=#, property_image=#
                            WHERE property_id=#";
        public const string PropertyDeactivateQry = "UPDATE mc_property SET is_deleted=1 WHERE property_id=#";
        #endregion


        #region HouseHold
        public const string HouseholdsQry = @"select 
                            mh.estate_id, mh.property_id, mh.household_id, mh.household_label, mh.status household_status, mh.remarks household_remarks, mh.usage_category, mh.household_classification, mh.entry_by, mh.entry_date, mh.last_modified_by, mh.last_modified_date, 
                            mp.estate_id, mp.property_id, mp.zipcode_id, mp.property_no, mp.property_type, mp.property_category, mp.status property_status, mp.remarks property_remarks, mp.owners_firstname, mp.owners_lastname, mp.owners_mobileno, mp.owners_email, mp.owners_gender, mp.owners_other_address, 
                            mz.zone_id, mz.zone_code, mz.zone_name, mz.zone_type,
                            ms.street_id, ms.street_code, ms.street_name, ms.street_type,
                            me.estate_name, me.estate_desc
                            -- , mr.resident_category, mr.resident_code, mr.resident_firstname, mr.resident_lastname, mr.resident_mobileno, mr.resident_emailaddress, mr.status resident_status
                            from mc_household mh
                            join mc_property mp on mp.property_id = mh.property_id
                            join mc_zipcode mzc on mzc.zipcode_id = mp.zipcode_id
                            join mc_zone mz on mz.zone_id = mzc.zone_id  
                            join mc_street ms on ms.street_id = mzc.street_id
                            join mc_estate me on me.estate_id = mp.estate_id 
                            -- left join mc_resident mr on mr.household_id = mh.household_id
                            where mh.estate_id like # and mh.property_id like # and (upper(mh.household_label) like # or mp.property_no like # or upper(ms.street_name) like #) and mz.zone_id like # and ms.street_id like # and mh.is_deleted = 0
                            order by mp.property_no, ms.street_name, mh.household_label 
                            limit # offset #";
        public const string HouseholdsQryCount = @"select count(1)
                            from mc_household mh
                            join mc_property mp on mp.property_id = mh.property_id
                            join mc_zipcode mzc on mzc.zipcode_id = mp.zipcode_id
                            join mc_zone mz on mz.zone_id = mzc.zone_id  
                            join mc_street ms on ms.street_id = mzc.street_id
                            join mc_estate me on me.estate_id = mp.estate_id 
                            where mh.estate_id like # and mh.property_id like # and (upper(mh.household_label) like # or mp.property_no like # or upper(ms.street_name) like #) and mz.zone_id like # and ms.street_id like # and mh.is_deleted = 0";
        public const string HouseholdByIdQry = @"select 
                            mh.estate_id, mh.property_id, mh.household_id, mh.household_label, mh.status household_status, mh.remarks household_remarks, mh.usage_category, mh.household_classification, mh.entry_by, mh.entry_date, mh.last_modified_by, mh.last_modified_date, 
                            mp.estate_id, mp.property_id, mp.zipcode_id, mp.property_no, mp.property_type, mp.property_category, mp.status property_status, mp.remarks property_remarks, mp.owners_firstname, mp.owners_lastname, mp.owners_mobileno, mp.owners_email, mp.owners_gender, mp.owners_other_address, 
                            mz.zone_id, mz.zone_code, mz.zone_name, mz.zone_type,
                            ms.street_id, ms.street_code, ms.street_name, ms.street_type,
                            me.estate_name, me.estate_desc
                            -- , mr.resident_category, mr.resident_code, mr.resident_firstname, mr.resident_lastname, mr.resident_mobileno, mr.resident_emailaddress, mr.status resident_status
                            from mc_household mh
                            join mc_property mp on mp.property_id = mh.property_id
                            join mc_zipcode mzc on mzc.zipcode_id = mp.zipcode_id
                            join mc_zone mz on mz.zone_id = mzc.zone_id  
                            join mc_street ms on ms.street_id = mzc.street_id
                            join mc_estate me on me.estate_id = mp.estate_id 
                            -- left join mc_resident mr on mr.household_id = mh.household_id
                            where mh.household_id = # and mh.is_deleted = 0";
        public const string HouseholdCreateQry = @"INSERT INTO 
                            mc_household (estate_id, property_id, household_id, household_label, status, remarks, usage_category, household_classification, entry_by, entry_date)
                            VALUES(#, #, #, #, #, #, #, #, #, #)";
        public const string HouseholdUpdateQry = @"UPDATE mc_household
                            SET estate_id=#, property_id=#, household_label=#, status=#, remarks=#, usage_category=#, household_classification=#, last_modified_by=#, last_modified_date=#
                            WHERE household_id=#";
        public const string HouseholdDeactivateQry = "UPDATE mc_household SET is_deleted=1 WHERE household_id=#";
        public const string GetHouseholdIdsByEstateId = "select household_id from mc_household mh where estate_id = #";
        public const string GetHouseholdIdsByZoneId = "select mh.household_id from mc_household mh, mc_property mp, mc_zipcode mzc, mc_zone mz where mh.property_id = mp.property_id and mp.zipcode_id = mzc.zipcode_id and mzc.zone_id = mz.zone_id and mh.estate_id = # and mz.zone_id = # and mz.zone_id not in ('000')";
        #endregion


        #region Street
        public const string StreetsQry = @"select 
                            ms.estate_id, ms.street_id, ms.street_code, ms.street_name, ms.street_type, ms.entry_by, ms.entry_date, ms.last_modified_by, ms.last_modified_date, 
                            mzc.zipcode_id,
                            mz.zone_id, mz.zone_code, mz.zone_name, mz.zone_type,
                            me.estate_name, me.estate_desc  
                            from mc_street ms
                            join mc_zipcode mzc on mzc.street_id = ms.street_id 
                            join mc_zone mz on mz.zone_id  = mzc.zone_id  
                            join mc_estate me on me.estate_id = ms.estate_id 
                            where (upper(ms.street_code) like # or upper(ms.street_name) like #) and ms.street_type like # and mz.zone_id like # and mz.estate_id like # and ms.is_deleted = 0
                            order by ms.street_name 
                            limit 10 offset 0";
        public const string StreetsQryCount = @"select count(1)
                            from mc_street ms 
                            join mc_zipcode mzc on mzc.street_id = ms.street_id 
                            join mc_zone mz on mz.zone_id  = mzc.zone_id  
                            join mc_estate me on me.estate_id = ms.estate_id 
                            where (upper(ms.street_code) like # or upper(ms.street_name) like #) and ms.street_type like # and mz.zone_id like # and mz.estate_id like # and ms.is_deleted = 0";
        public const string StreetByIdQry = @"
                            select ms.estate_id, ms.street_id, ms.street_code, ms.street_name, ms.street_type, ms.entry_by, ms.entry_date, ms.last_modified_by, ms.last_modified_date,
                            mzc.zipcode_id,
                            mz.zone_id, mz.zone_code, mz.zone_name, mz.zone_type,
                            me.estate_name, me.estate_desc  
                            from mc_street ms
                            join mc_zipcode mzc on mzc.street_id = ms.street_id 
                            join mc_zone mz on mz.zone_id  = mzc.zone_id  
                            join mc_estate me on me.estate_id = ms.estate_id
                            where ms.street_id = # and ms.is_deleted = 0";
        public const string StreetCreateQry = @"INSERT INTO 
                            mc_street (estate_id, street_id, street_code, street_name, street_type, entry_by, entry_date)
                            VALUES (#, #, #, #, #, #, #)";
        public const string StreetUpdateQry = @"UPDATE mc_street
                            SET street_code=#, street_name=#, street_type=#, last_modified_by=#, last_modified_date=#
                            WHERE street_id=#";
        public const string StreetDeactivateQry = "update mc_street set is_deleted=1 where street_id=#";

        #endregion


        #region Zipcode
        public const string ZipcodeCreateQry = @"INSERT INTO 
                            mc_zipcode (estate_id, zipcode_id, zone_id, street_id)
                            VALUES (#, #, #, #)";
        #endregion


        #region Visitor
        public const string VisitorBookingsQry = @"SELECT 
                            vb.estate_id, vb.zone_id, vb.booking_id, vb.booking_date, vb.booking_title, vb.booking_group_no, vb.resident_id, vb.visitors_name, vb.visitors_mobileno, vb.visitors_email, vb.is_accompanied, vb.accompanied_by, vb.access_code, vb.validity_startdate, vb.validity_enddate, vb.qrcode_text, vb.qrcode_image,
                            vb.access_starttime, vb.access_endtime, vb.passage_mode, vb.is_recurring, vb.recurring_monday, vb.recurring_tuesday, vb.recurring_wednesday, vb.recurring_thursday, vb.recurring_friday, vb.recurring_saturday, vb.recurring_sunday, vb.is_exit_clearance_required, vb.is_cleared_to_exit, vb.remarks,
                            me.estate_name, me.estate_desc,
                            mz.zone_name ,
                            mr.resident_firstname, mr.resident_lastname, mr.resident_mobileno, mr.resident_emailaddress, mr.resident_category  
                            FROM visitor_booking vb
                            join mc_estate me on me.estate_id = vb.estate_id 
                            join mc_zone mz on mz.zone_id = vb.zone_id 
                            join mc_resident mr on mr.resident_id = vb.resident_id 
                            where vb.estate_id like # and vb.zone_id like # and vb.resident_id like # and upper(vb.booking_title) like # and vb.is_recurring like # and vb.is_cleared_to_exit like #
                            order by vb.booking_date desc
                            limit # offset #";
        public const string VisitorBookingsByBookingGroupNoQry = @"SELECT 
                            vb.estate_id, vb.zone_id, vb.booking_id, vb.booking_date, vb.booking_title, vb.booking_group_no, vb.resident_id, vb.visitors_name, vb.visitors_mobileno, vb.visitors_email, vb.is_accompanied, vb.accompanied_by, vb.access_code, vb.validity_startdate, vb.validity_enddate, vb.qrcode_text, vb.qrcode_image,
                            vb.access_starttime, vb.access_endtime, vb.passage_mode, vb.is_recurring, vb.recurring_monday, vb.recurring_tuesday, vb.recurring_wednesday, vb.recurring_thursday, vb.recurring_friday, vb.recurring_saturday, vb.recurring_sunday, vb.is_exit_clearance_required, vb.is_cleared_to_exit, vb.remarks,
                            me.estate_name, me.estate_desc,
                            mz.zone_name ,
                            mr.resident_firstname, mr.resident_lastname, mr.resident_mobileno, mr.resident_emailaddress, mr.resident_category  
                            FROM visitor_booking vb
                            join mc_estate me on me.estate_id = vb.estate_id 
                            join mc_zone mz on mz.zone_id = vb.zone_id 
                            join mc_resident mr on mr.resident_id = vb.resident_id 
                            where vb.booking_group_no = #
                            order by vb.booking_date desc";
        public const string VisitorBookingByIdQry = @"SELECT 
                            vb.estate_id, vb.zone_id, vb.booking_id, vb.booking_date, vb.booking_title, vb.booking_group_no, vb.resident_id, vb.visitors_name, vb.visitors_mobileno, vb.visitors_email, vb.is_accompanied, vb.accompanied_by, vb.access_code, vb.validity_startdate, vb.validity_enddate, vb.qrcode_text, vb.qrcode_image,
                            vb.access_starttime, vb.access_endtime, vb.passage_mode, vb.is_recurring, vb.recurring_monday, vb.recurring_tuesday, vb.recurring_wednesday, vb.recurring_thursday, vb.recurring_friday, vb.recurring_saturday, vb.recurring_sunday, vb.is_exit_clearance_required, vb.is_cleared_to_exit, vb.remarks,
                            me.estate_name, me.estate_desc,
                            mz.zone_name ,
                            mr.resident_firstname, mr.resident_lastname, mr.resident_mobileno, mr.resident_emailaddress, mr.resident_category  
                            FROM visitor_booking vb
                            join mc_estate me on me.estate_id = vb.estate_id 
                            join mc_zone mz on mz.zone_id = vb.zone_id 
                            join mc_resident mr on mr.resident_id = vb.resident_id 
                            where vb.booking_id = #";
        public const string VisitorBookingByAccessCodeQry = @"SELECT 
                            vb.estate_id, vb.zone_id, vb.booking_id, vb.booking_date, vb.booking_title, vb.booking_group_no, vb.resident_id, vb.visitors_name, vb.visitors_mobileno, vb.visitors_email, vb.is_accompanied, vb.accompanied_by, vb.access_code, vb.validity_startdate, vb.validity_enddate, vb.qrcode_text, vb.qrcode_image,
                            vb.access_starttime, vb.access_endtime, vb.passage_mode, vb.is_recurring, vb.recurring_monday, vb.recurring_tuesday, vb.recurring_wednesday, vb.recurring_thursday, vb.recurring_friday, vb.recurring_saturday, vb.recurring_sunday, vb.is_exit_clearance_required, vb.is_cleared_to_exit, vb.remarks,
                            me.estate_name, me.estate_desc,
                            mz.zone_name ,
                            mr.resident_firstname, mr.resident_lastname, mr.resident_mobileno, mr.resident_emailaddress, mr.resident_category  
                            FROM visitor_booking vb
                            join mc_estate me on me.estate_id = vb.estate_id 
                            join mc_zone mz on mz.zone_id = vb.zone_id 
                            join mc_resident mr on mr.resident_id = vb.resident_id 
                            where vb.access_code = #";
        public const string VisitorBookingsCountQry = @"SELECT count(1)
                            FROM visitor_booking vb
                            join mc_estate me on me.estate_id = vb.estate_id 
                            join mc_zone mz on mz.zone_id = vb.zone_id 
                            join mc_resident mr on mr.resident_id = vb.resident_id 
                            where vb.estate_id like # and vb.zone_id like # and vb.resident_id like # and upper(vb.booking_title) like # and vb.is_recurring like # and vb.is_cleared_to_exit like #";
        public const string VisitorBookingCreateQry = @"INSERT INTO visitor_booking
                            (estate_id, zone_id, booking_id, booking_date, booking_title, booking_group_no, resident_id, visitors_name, visitors_mobileno, visitors_email, is_accompanied, accompanied_by, access_code, validity_startdate, validity_enddate, access_starttime, access_endtime, passage_mode, is_recurring, is_exit_clearance_required, is_cleared_to_exit, qrcode_text, qrcode_image)
                            VALUES(#, #, #, #, #, #, #, #, #, #, #, #, #, #, #, #, #, #, #, #, #, #, #)";
        public const string VisitorBookingRecurringMondayQry = "update visitor_booking set recurring_monday = # where booking_id = #";
        public const string VisitorBookingRecurringTuesdayQry = "update visitor_booking set recurring_tuesday = # where booking_id = #";
        public const string VisitorBookingRecurringWednesdayQry = "update visitor_booking set recurring_wednesday = # where booking_id = #";
        public const string VisitorBookingRecurringThursdayQry = "update visitor_booking set recurring_thursday = # where booking_id = #";
        public const string VisitorBookingRecurringFridayQry = "update visitor_booking set recurring_friday = # where booking_id = #";
        public const string VisitorBookingRecurringSaturdayQry = "update visitor_booking set recurring_saturday = # where booking_id = #";
        public const string VisitorBookingRecurringSundayQry = "update visitor_booking set recurring_sunday = # where booking_id = #";
        public const string ExitVisitorBookingQry = "update visitor_booking set is_cleared_to_exit = # where booking_id = #";
        public const string VisitorAccessLog = "INSERT INTO public.visitor_accesslog (estate_id, log_id, booking_id, entry_timestamp, exit_timestamp, gate_id, remarks) VALUES(#, #, #, #, #, #, #)";
        #endregion


        #region Billing
        //public const string BillingsQry = @"select distinct 
        //                     bi.estate_id, bi.zone_id, bi.invoice_id, bi.invoice_date, bi.invoice_title, bi.status invoice_status, bi.target_flag,
        //                     me.estate_name, me.estate_desc,
        //                     mhb.bill_id, mhb.household_id,  mhb.status bill_status, mhb.status_date,
        //                     mh.property_id, mh.household_label, mh.usage_category,
        //                     mr.resident_mobileno, mr.resident_emailaddress,  
        //                     (select sum(mhb2.item_cost) from mc_household_billitem mhb2 where mhb2.bill_id = mhb.bill_id) bill_amount
        //                     from billing_invoice bi 
        //                     join mc_estate me on me.estate_id = bi.estate_id
        //                     join mc_household_bill mhb on mhb.invoice_id = bi.invoice_id
        //                     join mc_household mh on mh.household_id = mhb.household_id 
        //                     join mc_resident mr on mr.household_id = mhb.household_id 
        //                     where mhb.household_id like #  and mhb.status like # and mr.resident_category = 'PRIMARY'
        //                     order by bi.invoice_date 
        //                     limit # offset #";
        public const string BillingsQry = @"select 
                            mhb.bill_id, mhb.invoice_id, mhb.household_id, mhb.status bill_status, mhb.status_date bill_status_date,
                            bi.estate_id, bi.zone_id, bi.invoice_date, bi.invoice_title, bi.status invoice_status, bi.target_flag,
                            me.estate_name, me.estate_desc,
                            mzn.zone_code, mzn.zone_name, mzn.zone_type,  
                            mh.property_id, mh.household_label, mh.usage_category,
                            mp.zipcode_id, mp.property_no, mp.property_type, mp.property_category,
                            ms.street_code, ms.street_name, ms.street_type,
                            mr.resident_firstname, mr.resident_lastname, mr.resident_mobileno, mr.resident_emailaddress,
                            (select sum(mhbi.item_cost) from mc_household_billitem mhbi where mhbi.bill_id = mhb.bill_id) bill_amount,
                            mhp.payment_id, mhp.payment_amount, mhp.payment_method, mhp.payment_gateway, mhp.acceptance_status, mhp.payment_date, mhp.acceptance_date, mhp.gateway_ref, mhp.receipt, mhp.note
                            from mc_household_bill mhb 
                            join billing_invoice bi on bi.invoice_id = mhb.invoice_id
                            left join billing_target bt on bt.household_id = mhb.household_id 
                            join mc_estate me on me.estate_id=bi.estate_id
                            left join mc_zone mzn on mzn.zone_id = bi.zone_id 
                            join mc_household mh on mh.household_id = mhb.household_id
                            join mc_property mp on mp.property_id = mh.property_id 
                            join mc_zipcode mz on mz.zipcode_id = mp.zipcode_id 
                            join mc_street ms on ms.street_id = mz.street_id
                            join mc_resident mr on mr.household_id = mhb.household_id
                            left join mc_household_payment mhp on mhp.transaction_reference = mhb.bill_id  
                            where mhb.invoice_id like # and mhb.household_id like # and mhb.status like # and bi.target_flag like # and mr.resident_category = 'PRIMARY'
                            order by bi.invoice_date 
                            limit # offset #";
        //public const string BillingByIdQry = @"select distinct 
        //                     bi.estate_id, bi.zone_id, bi.invoice_id, bi.invoice_date, bi.invoice_title, bi.status invoice_status, bi.target_flag,
        //                     me.estate_name, me.estate_desc,
        //                     mhb.bill_id, mhb.household_id,  mhb.status bill_status, mhb.status_date,
        //                     mh.property_id, mh.household_label, mh.usage_category,
        //                     mr.resident_mobileno, mr.resident_emailaddress,  
        //                     (select sum(mhb2.item_cost) from mc_household_billitem mhb2 where mhb2.bill_id = mhb.bill_id) bill_amount
        //                     from billing_invoice bi 
        //                     join mc_estate me on me.estate_id = bi.estate_id
        //                     join mc_household_bill mhb on mhb.invoice_id = bi.invoice_id
        //                     join mc_household mh on mh.household_id = mhb.household_id 
        //                     join mc_resident mr on mr.household_id = mhb.household_id 
        //                     where mhb.bill_id = # and mr.resident_category = 'PRIMARY'";
        public const string BillingByIdQry = @"select 
                            mhb.bill_id, mhb.invoice_id, mhb.household_id, mhb.status bill_status, mhb.status_date bill_status_date,
                            bi.estate_id, bi.zone_id, bi.invoice_date, bi.invoice_title, bi.status invoice_status, bi.target_flag,
                            me.estate_name, me.estate_desc,
                            mzn.zone_code, mzn.zone_name, mzn.zone_type,  
                            mh.property_id, mh.household_label, mh.usage_category,
                            mp.zipcode_id, mp.property_no, mp.property_type, mp.property_category,
                            ms.street_code, ms.street_name, ms.street_type,
                            mr.resident_firstname, mr.resident_lastname, mr.resident_mobileno, mr.resident_emailaddress,
                            (select sum(mhbi.item_cost) from mc_household_billitem mhbi where mhbi.bill_id = mhb.bill_id) bill_amount,
                            mhp.payment_id, mhp.payment_amount, mhp.payment_method, mhp.payment_gateway, mhp.acceptance_status, mhp.payment_date, mhp.acceptance_date, mhp.gateway_ref, mhp.receipt, mhp.note
                            from mc_household_bill mhb 
                            join billing_invoice bi on bi.invoice_id = mhb.invoice_id
                            left join billing_target bt on bt.household_id = mhb.household_id 
                            join mc_estate me on me.estate_id=bi.estate_id
                            left join mc_zone mzn on mzn.zone_id = bi.zone_id 
                            join mc_household mh on mh.household_id = mhb.household_id
                            join mc_property mp on mp.property_id = mh.property_id 
                            join mc_zipcode mz on mz.zipcode_id = mp.zipcode_id 
                            join mc_street ms on ms.street_id = mz.street_id
                            join mc_resident mr on mr.household_id = mhb.household_id
                            left join mc_household_payment mhp on mhp.transaction_reference = mhb.bill_id  
                            where mhb.bill_id = # and mr.resident_category = 'PRIMARY'";
        //public const string BillingsCountQry = @" 
        //                     select 
        //                     COUNT(1)
        //                     from billing_invoice bi 
        //                     join mc_estate me on me.estate_id = bi.estate_id
        //                     join mc_household_bill mhb on mhb.invoice_id = bi.invoice_id
        //                     join mc_household mh on mh.household_id = mhb.household_id 
        //                     join mc_resident mr on mr.household_id = mhb.household_id 
        //                     where mhb.household_id like # and mhb.status like # and mr.resident_category = 'PRIMARY'";
        public const string BillingsCountQry = @"select count(1)
                            from mc_household_bill mhb 
                            join billing_invoice bi on bi.invoice_id = mhb.invoice_id
                            left join billing_target bt on bt.household_id = mhb.household_id 
                            join mc_estate me on me.estate_id=bi.estate_id
                            left join mc_zone mzn on mzn.zone_id = bi.zone_id 
                            join mc_household mh on mh.household_id = mhb.household_id
                            join mc_property mp on mp.property_id = mh.property_id 
                            join mc_zipcode mz on mz.zipcode_id = mp.zipcode_id 
                            join mc_street ms on ms.street_id = mz.street_id
                            join mc_resident mr on mr.household_id = mhb.household_id
                            where mhb.invoice_id like # and mhb.household_id like # and mhb.status like # and bi.target_flag like # and mr.resident_category = 'PRIMARY'";
        public const string BillingItems = @"select 
                            mhbi.bill_id, mhbi.item_id, mhbi.item_description, mhbi.item_cost 
                            from mc_household_billitem mhbi 
                            where mhbi.bill_id = #";
        public const string BillingCreateQry = @"INSERT INTO 
                            mc_household_bill (bill_id, invoice_id, household_id, status, status_date)
                            VALUES (#, #, #, #, #)";
        public const string BillingItemCreateQry = @"INSERT INTO 
                            mc_household_billitem (bill_id, item_id, item_description, item_cost)
                            VALUES (#, #, #, #)";
        public const string BillingTargetCreateQry = @"INSERT INTO billing_target
                            (invoice_id, household_id)
                            VALUES(#, #)";
        #endregion


        #region Payments
        public const string MakePaymentsQry = @"INSERT INTO mc_household_payment
                            (estate_id, payment_id, household_id, transaction_reference, payment_amount, payment_desc, payment_method, payment_gateway, payment_date, gateway_ref, receipt, note, access_code)
                            VALUES(#, #, #, #, #, #, #, #, #, #, #, #, #)";
        public const string PaymentByReferenceQry = "select estate_id, payment_id, household_id, transaction_reference billId, payment_amount, payment_desc,  payment_method, payment_gateway,   acceptance_status,payment_date, gateway_ref, receipt, note   from mc_household_payment mhp where gateway_ref = #";
        public const string CloseHouseholdBill = "UPDATE mc_household_bill SET status=#, status_date=# WHERE bill_id=#";
        public const string CheckExistsGatewayRef = "select count(1) from mc_household_payment mhp where mhp.gateway_ref = #";
        public const string GetBillByGatewayRef = "select transaction_reference from mc_household_payment mhp where mhp.gateway_ref = #";
        public const string CheckAcceptedPaymentByGatewayRef = "select count(1) from mc_household_payment mhp where mhp.acceptance_status = # and mhp.gateway_ref = #";
        public const string VerifyPayment = "UPDATE mc_household_payment set acceptance_status = #, acceptance_date = # where gateway_ref = #";
        #endregion


        #region Zone
        public const string ZonesQry = @"select 
                            mz.estate_id, mz.zone_id, mz.zone_code, mz.zone_name, mz.zone_type, mz.entry_by, mz.entry_date, mz.last_modified_by, mz.last_modified_date,
                            me.estate_name, me.estate_desc, me.estate_code, me.image_url  
                            from  mc_zone mz
                            join mc_estate me on me.estate_id = mz.estate_id 
                            where mz.zone_id not in ('000') and mz.estate_id like # and (upper(mz.zone_code) like # or upper(mz.zone_name) like #) and mz.zone_type like # and mz.is_deleted = 0
                            order by mz.zone_name 
                            limit # offset #";
        public const string ZonesQryCount = @"
                            select count(1)
                            from  mc_zone mz
                            join mc_estate me on me.estate_id = mz.estate_id 
                            where mz.zone_id not in ('000') and mz.estate_id like # and (upper(mz.zone_code) like # or upper(mz.zone_name) like #) and mz.zone_type like # and mz.is_deleted = 0";
        public const string ZoneByIdQry = @"select 
                            mz.estate_id, mz.zone_id, mz.zone_code, mz.zone_name, mz.zone_type, mz.entry_by, mz.entry_date, mz.last_modified_by, mz.last_modified_date,
                            me.estate_name, me.estate_desc, me.estate_code, me.image_url  
                            from  mc_zone mz
                            join mc_estate me on me.estate_id = mz.estate_id 
                            where mz.zone_id = # and mz.is_deleted = 0";
        public const string ZoneCreateQry = @"INSERT INTO 
                            mc_zone(estate_id, zone_id, zone_code, zone_name, zone_type, entry_by, entry_date)
                            VALUES(#, #, #, #, #, #, #)";
        public const string ZoneUpdateQry = @"UPDATE mc_zone
                            SET estate_id=#, zone_code=#, zone_name=#, zone_type=#, last_modified_by=#, last_modified_date=#
                            WHERE zone_id=#";
        public const string ZoneDeactivateQry = "UPDATE mc_zone SET is_deleted=1 WHERE zone_id=#";
        #endregion


        #region Invoice
        public const string InvoicesQry = @"SELECT 
                            bi.estate_id, bi.zone_id, bi.invoice_id, bi.invoice_date, bi.invoice_title, bi.status, bi.target_flag,
                            me.estate_name, me.estate_desc, me.estate_code, me.image_url estate_image,
                            mz.zone_name
                            FROM billing_invoice bi
                            JOIN mc_estate me on me.estate_id = bi.estate_id 
                            LEFT JOIN mc_zone mz on mz.zone_id = bi.zone_id                             
                            WHERE bi.estate_id like # and bi.zone_id like # and upper(bi.invoice_title) like # and upper(bi.status) like # and upper(bi.target_flag) like #
                            order by bi.invoice_date desc 
                            limit # offset #";
        public const string InvoiceByIdQry = @"SELECT 
                            bi.estate_id, bi.zone_id, bi.invoice_id, bi.invoice_date, bi.invoice_title, bi.status, bi.target_flag,
                            me.estate_name, me.estate_desc, me.estate_code, me.image_url estate_image,
                            mz.zone_name
                            FROM billing_invoice bi
                            JOIN mc_estate me on me.estate_id = bi.estate_id 
                            LEFT JOIN mc_zone mz on mz.zone_id = bi.zone_id                             
                            WHERE bi.invoice_id = #";
        public const string InvoiceItemsQryByInvoiceId = @"SELECT bii.invoice_id, bii.invoice_item_id, bii.invoice_item_title, bii.cost_type, bii.flat_rate_cost, bii.billing_element_id
                            FROM billing_invoice_item bii
                            where bii.invoice_id = #";
        public const string InvoicesCountQry = @"SELECT COUNT(1)
                            FROM billing_invoice bi
                            JOIN mc_estate me on me.estate_id = bi.estate_id 
                            LEFT JOIN mc_zone mz on mz.zone_id = bi.zone_id                             
                            WHERE bi.estate_id like # and bi.zone_id like # and upper(bi.invoice_title) like # and upper(bi.status) like # and upper(bi.target_flag) like #";
        public const string InvoiceCreateQry = @"INSERT INTO 
                            billing_invoice (estate_id, zone_id, invoice_id, invoice_date, invoice_title, status, target_flag)
                            VALUES(#, #, #, #, #, #, #)";
        public const string InvoiceItemCreateQry = @"INSERT INTO billing_invoice_item
                            (invoice_id, invoice_item_id, invoice_item_title, cost_type, flat_rate_cost, billing_element_id)
                            VALUES(#, #, #, #, #, #)";

        #endregion



        #region Billing Elements
        public const string BillingElementsQry = @"SELECT 
                            be.estate_id, be.zone_id, be.element_id, be.element_title, be.status,
                            bec.classification_code, bec.cost,
                            me.estate_name,
                            mz.zone_name
                            FROM billing_element be
                            JOIN billing_element_cost bec on bec.element_id = be.element_id 
                            JOIN mc_estate me on me.estate_id = be.estate_id 
                            LEFT JOIN mc_zone mz on mz.zone_id = be.zone_id 
                            where be.estate_id like # and be.zone_id like # and upper(be.element_title) like # and upper(bec.classification_code) like # 
                            limit # offset #";
        public const string BillingElementByIdQry = @"SELECT 
                            be.estate_id, be.zone_id, be.element_id, be.element_title, be.status,
                            bec.classification_code, bec.cost,
                            me.estate_name,
                            mz.zone_name
                            FROM billing_element be
                            JOIN billing_element_cost bec on bec.element_id = be.element_id 
                            JOIN mc_estate me on me.estate_id = be.estate_id 
                            LEFT JOIN mc_zone mz on mz.zone_id = be.zone_id 
                            where be.element_id = #";
        public const string BillingElementsQryCount = @"SELECT count(1)
                            FROM billing_element be
                            JOIN billing_element_cost bec on bec.element_id = be.element_id 
                            JOIN mc_estate me on me.estate_id = be.estate_id 
                            LEFT JOIN mc_zone mz on mz.zone_id = be.zone_id 
                            where be.estate_id like # and be.zone_id like # and upper(be.element_title) like # and upper(bec.classification_code) like #";
        public const string BillingElementCreateQry = @"INSERT INTO 
                            billing_element (estate_id, zone_id, element_id, element_title, status)
                            VALUES(#, #, #, #, #)";
        public const string BillingElementCostCreateQry = @"INSERT INTO 
                            billing_element_cost (element_id, classification_code, cost)
                            VALUES(#, #, #)";
        #endregion


        #region CGF
        public const string UsageCategoriesQry = @"SELECT 
                            cuc.estate_id, cuc.usage_category_code, cuc.usage_category_name, cuc.status,
                            me.estate_name
                            FROM cfg_usage_category cuc
                            JOIN mc_estate me ON me.estate_id = cuc.estate_id
                            WHERE cuc.estate_id LIKE #";
        public const string UsageCategoryCreateQry = @"INSERT INTO 
                            cfg_usage_category (estate_id, usage_category_code, usage_category_name, status)
                            VALUES (#, #, #, #)";
        public const string UsageCategoryUpdateQry = @"UPDATE cfg_usage_category
                            SET usage_category_name=#, status=#
                            WHERE estate_id=# AND usage_category_code=#";

        public const string PropertyTypesQry = @"SELECT cpt.estate_id, cpt.property_type_code, cpt.property_type_name, cpt.status,
                            me.estate_name
                            FROM cfg_property_type cpt
                            JOIN mc_estate me ON me.estate_id = cpt.estate_id
                            WHERE cpt.estate_id LIKE #";
        public const string PropertyTypeCreateQry = @"INSERT INTO 
                            cfg_property_type (estate_id, property_type_code, property_type_name, status)
                            VALUES (#, #, #, #)";
        public const string PropertyTypeUpdateQry = @"UPDATE cfg_property_type
                            SET property_type_name=#, status=#
                            WHERE estate_id=# AND property_type_code=#";

        public const string HouseholdClassificationQry = @"SELECT 
                            chc.estate_id, chc.zone_id, chc.classification_code, chc.classification_name,
                            me.estate_name, mz.zone_name  
                            FROM cfg_household_classification chc
                            JOIN mc_estate me ON me.estate_id  = chc.estate_id 
                            JOIN mc_zone mz ON mz.zone_id = chc.zone_id 
                            WHERE chc.estate_id LIKE # AND chc.zone_id LIKE #";
        public const string HouseholdClassificationCreateQry = @"INSERT INTO 
                            cfg_household_classification (estate_id, zone_id, classification_code, classification_name)
                            VALUES (#, #, #, #)";
        public const string HouseholdClassificationUpdateQry = @"UPDATE cfg_household_classification
                            SET classification_name=#
                            WHERE estate_id=# AND zone_id=# AND classification_code=#";
        #endregion
    }
}
