import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule }   from '@angular/forms';     
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FlexLayoutModule } from '@angular/flex-layout';
import { CommonModule } from '@angular/common';
import { DictionaryPipe } from 'app/Base/dictionary';
import { SanitizeHtml } from 'app/Base/SanitizeHtml.pipe';
import { CustomMaterialModule } from 'app/custom-material.module';
import {    
    DialogContent,
    UIMessage
} from './Reporting/Reporting1095/reporting1095';


@NgModule({
    imports:
        [
            CustomMaterialModule,
            CommonModule,
            BrowserModule,
            BrowserAnimationsModule,
            FormsModule,         
            FlexLayoutModule        
        ],
    declarations:
        [
            DialogContent, UIMessage, DictionaryPipe, SanitizeHtml
        ],
    exports:
        [
            CustomMaterialModule,
            CommonModule,
            BrowserModule,
            BrowserAnimationsModule,
            FormsModule, 
            FlexLayoutModule,
            DictionaryPipe,
            SanitizeHtml,
            DialogContent,
            UIMessage
        ],
    bootstrap: [DialogContent, UIMessage]
})

export class SharedModule { }
