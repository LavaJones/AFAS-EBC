$(function() {
    /*
	number of fieldsets
	*/
    var fieldsetCount = 4;
    /*
	current position of fieldset / navigation link
	*/
    var current = 1;
    var prev = 1;

    /********************************************************************************
    *************START: Tab 1 Blur.Focus Actions ************************************
    ********************************************************************************/
    //Item 1: Employer Information
    $('#DdlEmployerType').focus(
       function () {
           $(this).css({ 'background': 'yellow' });
           $('#ProfileMessage').text('Select your Employer Type.');
       });
    
    $('#DdlEmployerType').blur(
        function () {
            checkEmployerType(null);
        });

    //Item 2: Employer Name
    $('#TxtDistName').blur(
        function () {
            checkDistrictName(null);
        });
    $('#TxtDistName').focus(
        function () {
            $(this).css({ 'background': 'yellow' });
            $('#ProfileMessage').text('Enter the Employer Name.');
        });

    //Item 2: Employer Payroll Company
    $('#TxtEmployerPayrollSoftware').blur(
        function () {
            checkPayrollName(null);
        });
    $('#TxtEmployerPayrollSoftware').focus(
        function () {
            $(this).css({ 'background': 'yellow' });
            $('#ProfileMessage').text('Enter the name of the payroll system your business uses.');
        });

    //Item 3: Employer EIN Number
    $('#TxtEmployerEIN').blur(
        function () {
            checkEmployerEIN(null);
        });
    $('#TxtEmployerEIN').focus(
        function () {
            $(this).css({ 'background': 'yellow' });
            $('#ProfileMessage').text('Enter the Employer EIN Number. Format: xx-xxxxxxx');
        });

    //Item 4: District Address
    $('#TxtDistAddress').blur(
        function () {
            checkDistrictAddress(null);
        });
    $('#TxtDistAddress').focus(
        function () {
            $(this).css({ 'background': 'yellow' });
            $('#ProfileMessage').text('Enter the Employer Address.');
        });

    //Item 5: District City
    $('#TxtDistCity').blur(
        function () {
            checkDistrictCity(null);
        });
    $('#TxtDistCity').focus(
        function () {
            $(this).css({ 'background': 'yellow' });
            $('#ProfileMessage').text('Enter the Employer City.');
        });

    //Item 6: District State
    $('#DdlDistState').focus(
       function () {
           $(this).css({ 'background': 'yellow' });
           $('#ProfileMessage').text('Select the Employer State.');
       });

    $('#DdlDistState').blur(
        function () {
            checkDistrictState(null);
        });

    //Item 7: District Zip Code
    $('#TxtDistZip').blur(
        function () {
            checkDistrictZip(null);
        });
    $('#TxtDistZip').focus(
        function () {
            $(this).css({ 'background': 'yellow' });
            $('#ProfileMessage').text('Enter the Employer Zip Code.');
        });
   
    //Item 8: Plan Year Renewal Description
    $('#TxtDistRenewalDescription').blur(
        function () {
                checkDistrictRenewalDescription(null);
        });
    $('#TxtDistRenewalDescription').focus(
        function () {
            $(this).css({ 'background': 'yellow' });
            $('#ProfileMessage').text('Enter a description for the next Plan Renewal Date.');
        });

    //Item 9: Plan Year Renewal Date for 1st Plan Year
    $('#DdlRenewalDate1').blur(
        function () {
            checkRenewalDate();
        });
    $('#DdlRenewalDate1').focus(
        function () {
            $(this).css({ 'background': 'yellow' });
            $('#ProfileMessage').text('Select the month that your plan will renew in 2015.');
        });


    //Item 11: Plan Year Renewal Date for 2nd Plan Year
    $('#DdlRenewalDate2').blur(
        function (){
        checkRenewalDate2();
        });
    $('#DdlRenewalDate2').focus(
        function () {
            $(this).css({ 'background': 'yellow' });
            $('#ProfileMessage').text('Select the month that your 2nd plan will renew in 2015.');
        });

    //Item 12: Plan Year Renewal Description
    $('#TxtDistRenewalDescription2').blur(
        function () {
            checkDistrictRenewalDescription2(null);
        });
    $('#TxtDistRenewalDescription2').focus(
        function () {
            $(this).css({ 'background': 'yellow' });
            $('#ProfileMessage').text('Enter a description for the next Plan Renewal Date.');
        });

    //Item 13: If the District has a second Plan Year.
    //alert($("input:radio[name='hi']:checked").val());
    $('#TxtUserFname').blur(
        function () {
            checkUserFname(null);
        });
    $('#TxtUserFname').focus(
        function () {
            $(this).css({ 'background': 'yellow' });
            $('#ProfileMessage').text('Enter the FIRST NAME of the person that will manage this software.');
        });

    $('#TxtUserLname').blur(
        function () {
            checkUserLname(null);
        });
    $('#TxtUserLname').focus(
        function () {
            $(this).css({ 'background': 'yellow' });
            $('#ProfileMessage').text('Enter the LAST NAME of the person that will manage this software.');
        });

    $('#TxtUserEmail').blur(
        function () {
            checkUserEmail(null);
        });
    $('#TxtUserEmail').focus(
        function () {
            $(this).css({ 'background': 'yellow' });
            $('#ProfileMessage').text('Enter the EMAIL ADDRESS of the person that will manage this software.');
        });

    $('#TxtUserPhone').blur(
       function () {
           checkUserPhone(null);
       });
    $('#TxtUserPhone').focus(
        function () {
            $(this).css({ 'background': 'yellow' });
            $('#ProfileMessage').text('Enter the PHONE NUMBER of the person that will manage this software.');
        });

    $('#TxtUserName').blur(
      function () {
          checkUsername(null);
      });
    $('#TxtUserName').focus(
        function () {
            $(this).css({ 'background': 'yellow' });
            $('#ProfileMessage').text('Enter the USERNAME of the person that will manage this software. (6 character minimum)');
        });

    $('#TxtUserPass').blur(
      function () {
          checkInitPassword(null);
      });
    $('#TxtUserPass').focus(
        function () {
            $(this).css({ 'background': 'yellow' });
            $('#ProfileMessage').text('Enter the PASSWORD of the person that will manage this software.\r (6-12 characters), (1 lower case), (1 upper case), (1 digit), (1 special character).');
        });

    $('#TxtUserPass2').blur(
     function () {
         checkBothPasswords(null);
     });
    $('#TxtUserPass2').focus(
        function () {
            $(this).css({ 'background': 'yellow' });
            $('#ProfileMessage').text('Re-enter the password.');
        });

    $('#TxtTab1').focus(
        function () {
            $('#TxtDistName').focus();
        });

    $('#TxtTab4').focus(
        function () {
            $('#TxtBillAddress').focus();
        });
   
   /********************************************************************************
   *************START: Tab 2 Blur.Focus Actions ************************************
   ********************************************************************************/
    $('#TxtBillAddress').blur(
     function () {
         checkBillAddress(null);
     });
    $('#TxtBillAddress').focus(
        function () {
            $(this).css({ 'background': 'yellow' });
            $('#ProfileMessage').text('Enter the ADDRESS of the billing location.');
        });

    $('#TxtBillCity').blur(
     function () {
         checkBillCity(null);
     });
    $('#TxtBillCity').focus(
        function () {
            $(this).css({ 'background': 'yellow' });
            $('#ProfileMessage').text('Enter the CITY of the billing location.');
        });

    $('#DdlBillState').focus(
       function () {
           $(this).css({ 'background': 'yellow' });
           $('#ProfileMessage').text('Select the STATE of the billing location.');
       });
    $('#DdlBillState').blur(
        function () {
            checkBillState(null);
        });

    $('#TxtBillZip').blur(
       function () {
           checkBillZip(null);
       });
    $('#TxtBillZip').focus(
        function () {
            $(this).css({ 'background': 'yellow' });
            $('#ProfileMessage').text('Enter the ZIP of the billing location.');
        });

    $('#TxtBillFName').blur(
    function () {
        checkBillFName(null);
    });
    $('#TxtBillFName').focus(
        function () {
            $(this).css({ 'background': 'yellow' });
            $('#ProfileMessage').text('Enter the FIRST NAME of the person that will manage the billing.');
        });

    $('#TxtBillLName').blur(
     function () {
         checkBillLName(null);
     });
    $('#TxtBillLName').focus(
        function () {
            $(this).css({ 'background': 'yellow' });
            $('#ProfileMessage').text('Enter the LAST NAME of the person that will manage the billing.');
        });

    $('#TxtBillEmail').blur(
       function () {
           checkBillEmail(null);
       });
    $('#TxtBillEmail').focus(
        function () {
            $(this).css({ 'background': 'yellow' });
            $('#ProfileMessage').text('Enter the EMAIL of the billing location.');
        });

    $('#TxtBillPhone').blur(
       function () {
           checkBillPhone(null);
       });

    $('#TxtBillPhone').focus(
        function () {
            $(this).css({ 'background': 'yellow' });
            $('#ProfileMessage').text('Enter the PHONE of the billing location.');
        });

    $('#TxtBillUsername').blur(
       function () {
           checkBillUsername(null);
       });

    $('#TxtBillUsername').focus(
        function () {
            $(this).css({ 'background': 'yellow' });
            $('#ProfileMessage').text('Enter the USERNAME of the person that will manage the billing. (6 character minimum)');
        });

    $('#TxtBillPassword').blur(
      function () {
          checkBillPassword(null);
      });
    $('#TxtBillPassword').focus(
        function () {
            $(this).css({ 'background': 'yellow' });
            $('#ProfileMessage').text('Enter the PASSWORD of the person that will manage the billing.\r (6-12 characters), (1 lower case), (1 upper case), (1 digit), (1 special character).');
        });

    $('#TxtBillPassword2').blur(
     function () {
         checkBothBillPasswords(null);
     });
    $('#TxtBillPassword2').focus(
        function () {
            $(this).css({ 'background': 'yellow' });
            $('#ProfileMessage').text('Re-enter the password.');
        });

    $('#TxtTab2').focus(
       function () {
           $('#TxtUserFname').focus();
       });

    $('#TxtTab3').focus(
       function () {
           $('#TxtBillFName').focus();
       });

    /********************************************************************************
    *************END: All BLUR/FOCUS functions for textboxes **********************************
    ********************************************************************************/
   
    /*
	sum and save the widths of each one of the fieldsets
	set the final sum as the total width of the steps element
	*/
    var stepsWidth	= 0;
    var widths 		= new Array();
    $('#steps .step').each(function(i){
        var $step 		= $(this);
        widths[i]  		= stepsWidth;
        stepsWidth	 	+= $step.width(); /*The + # is an adjustment to the slide width.*/
    });

    $('#steps').width(stepsWidth);
	
	
    /*
	show the navigation bar
	*/
    $('#navigation').show();
	
    /**************************************************************************************
    *********************** Slider Navigation Next/Pref Start *****************************
    ***************************************************************************************
    **************************************************************************************/
    $('#sliderPrev span').click(function (e) {
        if (current != 1)
        {
            next_slide = current - 1;
            prev = current;
            current -= 1;
            $('#ProfileMessage').text('');
            animate_slider();
        }
    });

    
    $('#sliderNext span').click(function (e) {
        if (current != fieldsetCount) {
            next_slide = current + 1;
            prev = current;
            current += 1;
            $('#ProfileMessage').text('');
            animate_slider();
        }
    });

    /**************************************************************************************
   *********************** Controls the left and right movement ***************************
   ***************************************************************************************
   **************************************************************************************/
    function animate_slider() {
        $('#steps').stop().animate({
            marginLeft: '-' + widths[current - 1] + 'px'
        }, 1000, function () {
            
            if (current == fieldsetCount) {
                validateSteps();
            }
            else {
                validateStep(prev);
            }
        });
    }
    /**************************************************************************************
    *********************** Slider Navigation Next/Prev End *******************************
    **************************************************************************************/
    /*
	when clicking on a navigation link 
	the form slides to the corresponding fieldset
	*/
    $('#navigation a').bind('click', function (e) {
        $('#ProfileMessage').text('');
        var $this	= $(this);
        prev = current;
        $this.closest('ul').find('li').removeClass('selected');
        $this.parent().addClass('selected');
        /*
		we store the position of the link
		in the current variable	
		*/
        current = $this.parent().index() + 1;
        animate_slider();

        e.preventDefault();
    });
	
	
	
    /*
	validates errors on all the fieldsets
	records if the Form has errors in $('#formElem').data()
	*/
    function validateSteps(){
        var FormErrors = false;
        for (var i = 1; i <= fieldsetCount; ++i)
        {
            var error = validateStep(i);
            if(error == -1)
                FormErrors = true;
        }
        $('#formElem').data('errors',FormErrors);	
    }
	
    /*
	validates one fieldset
	and returns -1 if errors found, or 1 if not
	*/
    function validateStep(step)
    {
        var error = 1;
        var hasError = false;
		
        if (step == 1)
        {
            var testValue = checkStep1();
            if (testValue == false)
            {
                hasError = true;
            }
        }

        if (step == 2)
        {
            var testValue = checkStep2();
            if (testValue == false)
            {
                hasError = true;
            }
        }

        if (step == 3) {
            var testValue = checkStep3();
            if (testValue == false) {
                hasError = true;
            }
        }

        if (step == 4)
        {
            var step1 = checkStep1();
            var step2 = checkStep2();
            var step3 = checkStep3();

            if (step1 == true || step2 == true || step3 == true) {
                $("#ImgBtnSubmit").attr('src', '../images/submit_false.png');
                //$('#redirectRelius').hide();
                $('#ProfileMessage').text('Please verify all tabs with a red X on them! When you have completed those steps there will be large green check mark on this page.');
                hasError = true;
            }
            else {
                $("#ImgBtnSubmit").attr('src', '../images/submit_ok.png');
                $('#ProfileMessage').text('Click the Green check mark to submit application.');
                hasError = false;
            }
            
            //Setting this to -1 will keep the form from submitting until everything is correct.
            if(hasError == true){
                error = -1;
            }
		
            return error;
        }
        }

        /***************************************************************************************************
        *************** All data validation for step 1 *****************************************************
        ***************************************************************************************************/
    //Employer Profile
    function checkStep1()
        {
            var validData = true;

            //alert('Call 1: ' + validData);
            validData = checkEmployerType(validData);               //Validate teh employer type.
            //alert('Call 2: ' + validData);
            validData = checkPayrollName(validData);                //Validate the Payroll Business Name.
            //alert('Call 3: ' + validData);
            validData = checkDistrictName(validData);               //Validate the employer name.
            //alert('Call 4: ' + validData);
            validData = checkEmployerEIN(validData);                //Validate the employer EIN number.
            //alert('Call 5: ' + validData);
            validData = checkDistrictAddress(validData);            //Validate the employer address.
            //alert('Call 6: ' + validData);
            validData = checkDistrictCity(validData);               //Validate the employer city.
            validData = checkDistrictZip(validData);                //Validate the employer zip code.
            validData = checkDistrictState(validData);              //Validate the employer state.
            validData = checkDistrictRenewalDescription(validData); //Validate the initial renewal date.
            validData = checkRenewalDate(validData);                //Validate the initial rewewal date.
            validData = check2ndRenewalInfo(validData);             //Validate the entire second renewal date.

            //alert('Step 1 End ' + validData);

            /*********************************************************************************************
            *********** Return whether the data is valid or not for STEP 1 *******************************
            *********************************************************************************************/
            if (validData == false)
            {
                $('#sp_step1').removeClass('error checked');
                $('#sp_step1').addClass('error');
                return true;
            }
            else {
                $('#sp_step1').removeClass('error checked');
                $('#sp_step1').addClass('checked');
                return false;
            }
        }

    //Username and Password Validation.
        function checkStep2()
        {
            var validData = true;

            //alert("Alert Data 1:" + validData);
            validData = checkUserFname(validData);
            //alert("Alert Data 2:" + validData);
            validData = checkUserLname(validData);
            //alert("Alert Data 3:" + validData);
            validData = checkUserEmail(validData);
            //alert("Alert Data 4:" + validData);
            validData = checkUserPhone(validData);
            //alert("Alert Data 5:" + validData);
            validData = checkUsername(validData);
            //alert("Alert Data 6:" + validData);
            validData = checkInitPassword(validData);
            //alert("Alert Data 7:" + validData);
            validData = checkBothPasswords(validData);
            //alert("Alert Data 8:" + validData);

            if (validData == false) {
                $('#sp_step2').removeClass('error checked');
                $('#sp_step2').addClass('error');
                return true;
            }
            else {
                $('#sp_step2').removeClass('error checked');
                $('#sp_step2').addClass('checked');
                return false;
            }
        }

        function checkStep3()
        {
            var validData = true;

            validData = checkBillAddress(validData);
            validData = checkBillCity(validData);
            validData = checkBillState(validData);
            validData = checkBillZip(validData);
            validData = checkBillingAdmin(validData);
           

            /*********************************************************************************************
            *********** Return whether the data is valid or not for STEP 1 *******************************
            *********************************************************************************************/
            if (validData == false) {
                $('#sp_step3').removeClass('error checked');
                $('#sp_step3').addClass('error');
                return true;
            }
            else {
                $('#sp_step3').removeClass('error checked');
                $('#sp_step3').addClass('checked');
                return false;
            }
        }

        function checkStep4()
        {
            var step1 = checkStep1();
            var step2 = checkStep2();
            var step3 = checkStep3();

            if (step1 == false || step2 == false || step3 == false) {
                $('#sp_step4').removeClass('error checked');
                $('#sp_step4').addClass('error');
                return true;
            }
            else {
                $('#sp_step4').removeClass('error checked');
                $('#sp_step4').addClass('checked');
                return false;
            }
        }

        function getTimeStamp()
        {
            var currentDateTime = new Date();
            var month = (currentDateTime.getMonth() + 1);
            var day = currentDateTime.getDate();
            var year = currentDateTime.getFullYear();
            var hour = currentDateTime.getHours();
            var min = currentDateTime.getMinutes();
            var sec = currentDateTime.getSeconds();


            if (min < 10)
            {
                min = '0' + min;
            }

            if (day < 10)
            {
                day = '0' + day;
            }

            if (month < 10)
            {
                month = '0' + month;
            }

            if (hour < 10)
            {
                hour = '0' + hour;
            }

            if (sec < 10)
            {
                sec = '0' + sec;
            }

            var tempdatetime = month + '/' + day + '/' + year + ' ' + hour + ':' + min + ':' + sec;
            return tempdatetime;
        }

        /***********************************************************************************************
        ************** When user tries to submit the form, display error message if not valid **********
        ***********************************************************************************************/
        function checkDistrictName(validData)
        {
            var test = validData;
            var input = $('#TxtDistName').val();
            var obj = $('#TxtDistName');
            var obj2 = $('#s_district');
            //alert('Pass in value: ' + test);

            validData = validEntry(obj, input, validData);
            passData(obj2, input);

            return validData;
        }

        function checkPayrollName(validData)
        {
            var input = $('#TxtEmployerPayrollSoftware').val();
            var obj = $('#TxtEmployerPayrollSoftware');

            validData = validEntry(obj, input, validData);

            return validData;
        }

        function checkEmployerEIN(validData) {
            var input = $('#TxtEmployerEIN').val();
            var obj = $('#TxtEmployerEIN');
            var obj2 = $('#s_employer_ein');
            validData = validEIN(obj, input, validData);
            passData(obj2, input);

            return validData;
        }

        function checkDistrictAddress(validData) {
            var input = $('#TxtDistAddress').val();
            var obj = $('#TxtDistAddress');
            var obj2 = $('#s_address');

            validData = validEntry(obj, input, validData);
            passData(obj2, input);

            return validData;
        }

        function checkDistrictCity(validData) {
            var input = $('#TxtDistCity').val();
            var obj = $('#TxtDistCity');
            var obj2 = $('#s_city');

            validData = validEntry(obj, input, validData);
            passData(obj2, input);

            return validData;
        }

        function checkDistrictState(validData) {
            var input = $('#DdlDistState').val();
            var ddlText = $('#DdlDistState option:selected').text();
            var obj = $('#DdlDistState');
            validData = validNumber(obj, input, validData);

            $('#s_state').text(ddlText);

            return validData;
        }

        function checkEmployerType(validData) {
            var input = $('#DdlEmployerType').val();
            var ddlText = $('#DdlEmployerType option:selected').text();
            var obj = $('#DdlEmployerType');
            validData = validNumber(obj, input, validData);

            $('#s_state').text(ddlText);

            return validData;
        }

        function checkDistrictZip(validData) {
            var input = $('#TxtDistZip').val();
            var obj = $('#TxtDistZip');
            var obj2 = $('#s_zip');

            validData = validZipCode(obj, input, validData);
            passData(obj2, input);

            return validData;
        }

        function checkUserFname(validData) {
            var input = $('#TxtUserFname').val();
            var obj = $('#TxtUserFname');
            var obj2 = $('#s_fname');

            validData = validEntry(obj, input, validData);
            passData(obj2, input);

            return validData;
        }

        function checkUserLname(validData) {
            var input = $('#TxtUserLname').val();
            var obj = $('#TxtUserLname');
            var obj2 = $('#s_lname');

            validData = validEntry(obj, input, validData);
            passData(obj2, input);

            return validData;
        }

        function checkUserEmail(validData) {
            var input = $('#TxtUserEmail').val();
            var obj = $('#TxtUserEmail');
            var obj2 = $('#s_email');

            validData = validEmail(obj, input, validData);
            passData(obj2, input);

            return validData;
        }

        function checkUserPhone(validData) {
            var input = $('#TxtUserPhone').val();
            var obj = $('#TxtUserPhone');
            var obj2 = $('#s_phone');

            validData = validPhone(obj, input, validData);
            passData(obj2, input);

            return validData;
        }

        function checkUsername(validData) {
            var input = $('#TxtUserName').val();
            var obj = $('#TxtUserName');

            validData = validUsername(obj, input, validData);
            passData(obj, input);

            return validData;
        }

        function checkBillUsername(validData) {
            var input = $('#TxtBillUsername').val();
            var obj = $('#TxtBillUsername');

            validData = validUsername(obj, input, validData);
            passData(obj, input);

            return validData;
        }

        function checkInitPassword(validData) {
            var input = $('#TxtUserPass').val();
            var obj = $('#TxtUserPass');

            validData = validInitialPassword(obj, input, validData);

            return validData;
        }

        function checkBillPassword(validData) {
            var input = $('#TxtBillPassword').val();
            var obj = $('#TxtBillPassword');

            validData = validInitialPassword(obj, input, validData);

            return validData;
        }


        function checkBothPasswords(validData) {
            var input = $('#TxtUserPass').val();
            var obj = $('#TxtUserPass');

            var input2 = $('#TxtUserPass2').val();
            var obj2 = $('#TxtUserPass2');
            validData = validPassword(obj, input, obj2, input2, validData);

            return validData;
        }

        function checkBothBillPasswords(validData) {
            var input = $('#TxtBillPassword').val();
            var obj = $('#TxtBillPassword');

            var input2 = $('#TxtBillPassword2').val();
            var obj2 = $('#TxtBillPassword2');
            validData = validPassword(obj, input, obj2, input2, validData);

            return validData;
        }

        function checkBillFName(validData) {
            var input = $('#TxtBillFName').val();
            var obj = $('#TxtBillFName');
            var obj2 = $('#b_fname');
            var value = true;

            validData = validEntry(obj, input, validData);
            passData(obj2, input);

            return validData;
        }

        function checkBillLName(validData) {
            var input = $('#TxtBillLName').val();
            var obj = $('#TxtBillLName');
            var obj2 = $('#b_lname');
            var value = true;

            validData = validEntry(obj, input, validData);
            passData(obj2, input);

            return validData;
        }

        function checkBillAddress(validData) {
            var input = $('#TxtBillAddress').val();
            var obj = $('#TxtBillAddress');
            var obj2 = $('#b_address');
            var value = true;

            validData = validEntry(obj, input, validData);
            passData(obj2, input);

            return validData;
        }

        function checkBillCity(validData) {
            var input = $('#TxtBillCity').val();
            var obj = $('#TxtBillCity');
            var obj2 = $('#b_city');
            var value = true;

            validData = validEntry(obj, input, validData);
            passData(obj2, input);

            return validData;
        }

        function checkBillState(validData) {
            var input = $('#DdlBillState').val();
            var ddlText = $('#DdlBillState option:selected').text();
            var obj = $('#DdlBillState');
            validData = validNumber(obj, input, validData);

            $('#b_state').text(ddlText);

            return validData;
        }


        function checkBillZip(validData) {
            var input = $('#TxtBillZip').val();
            var obj = $('#TxtBillZip');
            var obj2 = $('#b_zip');

            validData = validZipCode(obj, input, validData);
            passData(obj2, input);

            return validData;
        }

        function checkBillEmail(validData) {
            var input = $('#TxtBillEmail').val();
            var obj = $('#TxtBillEmail');
            var obj2 = $('#b_email');

            validData = validEmail(obj, input, validData);
            passData(obj2, input);

            return validData;
        }

        function checkBillPhone(validData) {
            var input = $('#TxtBillPhone').val();
            var obj = $('#TxtBillPhone');
            var obj2 = $('#b_phone');

            validData = validPhone(obj, input, validData);
            passData(obj2, input);

            return validData;
        }

        function checkDistrictRenewalDescription(validData) {
            var input = $('#TxtDistRenewalDescription').val();
            var obj = $('#TxtDistRenewalDescription');
            var obj2 = $('#s_insurance_name');

            validData = validEntry(obj, input, validData);
            passData(obj2, input);

            return validData;
        }

        function checkRenewalDate(validData) {
            var input = $('#DdlRenewalDate1').val();
            var rdate = input + "/1/2015";
            var ddlText = $('#DdlRenewalDate1 option:selected').text();
            var obj = $('#DdlRenewalDate1');
            var obj2 = $('#s_insurance');
            validData = validNumber(obj, input, validData);
            passData(obj2, rdate);
            
            return validData;
        }

        function check2ndRenewalInfo(validData)
        {
            var value = $("input:radio[name='hi']:checked").val();
            if (value == "RbtnYes")
            {
                validData = checkDistrictRenewalDescription2(validData);
                validData = checkRenewalDate2(validData);
                return validData;
            }
            else
            {
                var obj = $('#s_insurance2');
                var obj2 = $('#s_insurance_name2');
                passData(obj, " ");
                passData(obj2, " ");
                return validData;
            }
        }

        function checkDistrictRenewalDescription2(validData) {
            var input = $('#TxtDistRenewalDescription2').val();
            var obj = $('#TxtDistRenewalDescription2');
            var obj2 = $('#s_insurance_name2');

            validData = validEntry(obj, input, validData);
            passData(obj2, input);

            return validData;
        }

        function checkRenewalDate2(validData) {
            var input = $('#DdlRenewalDate2').val();
            var rdate = input + "/1/2015";
            var ddlText = $('#DdlRenewalDate2 option:selected').text();
            var obj = $('#DdlRenewalDate2');
            var obj2 = $('#s_insurance2');
            validData = validNumber(obj, input, null);
            passData(obj2, rdate);

            return validData;
        }

        function checkBillingAdmin(validData) {
            var value = $("input:radio[name='bi']:checked").val();
            if (value == "RbtnBillNo") {
                var input = $('#TxtBillFName').val();
                var input2 = $('#TxtBillLName').val();
                var obj2 = $('#b_fname');
                var obj3 = $('#b_lname');

                validData = validEntry(obj2, input, validData);
                validData = validEntry(obj3, input, validData);
                validData = checkBothBillPasswords(validData);
                validData = checkBillUsername(validData);

                passData(obj2, input);
                passData(obj3, input2);
                return validData;
            }
            else {
                //alert("Show Software Admin");
                var input = $('#TxtUserFname').val();
                var input2 = $('#TxtUserLname').val();
                var obj2 = $('#b_fname');
                var obj3 = $('#b_lname');

                validData = validEntry(obj2, input, validData);
                validData = validEntry(obj3, input, validData);

                passData(obj2, input);
                passData(obj3, input2);

                return validData;
            }
        }


        function passData(obj, value)
        {
            obj.text(value);
        }

        /***********************************************************************************************
        ************** When user tries to submit the form, display error message if not valid **********
        ***********************************************************************************************/
        $('#ImgBtnSubmit').bind('click', function () {
            var formError = $('#formElem').data('errors');

            if (formError == true)
            {
                alert('Please correct the errors in the Form');
                return false;
            }	
        });




});



