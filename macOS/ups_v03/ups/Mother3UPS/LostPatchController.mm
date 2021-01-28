//
//  LostPatchController.m
//  Mother3UPS
//

#import "LostPatchController.h"
#include "libups.hpp"

@implementation LostPatchController
- (void)showDialog{
    [_wndPatcher makeKeyAndOrderFront:nil];
}

- (IBAction)btnLostApply:(id)sender{
    NSFileManager *fileManager = [NSFileManager defaultManager];
    NSString *romPath = [_txtRomPath stringValue];
    NSString *backupPath = [romPath stringByAppendingString:@".bak"];
    NSString* patchPath = [_txtPatchPath stringValue];
    UPS ups; //UPS Patcher
    
    if([fileManager fileExistsAtPath:patchPath]){
        if([fileManager movePath:romPath toPath:backupPath handler:nil]){ //Make the backup by renaming the original file.
            [NSApp beginSheet:_pnlPatching modalForWindow:_wndPatcher modalDelegate:nil didEndSelector:nil contextInfo:nil]; //Make a sheet
            [_pbProgress setUsesThreadedAnimation:YES]; //Make sure it animates.
            [_pbProgress startAnimation:self];
            bool result = ups.apply([backupPath cString], [romPath cString], [patchPath cString]);
            [_pbProgress stopAnimation:self];
            [NSApp endSheet:_pnlPatching]; //Tell the sheet we're done.
            [_pnlPatching orderOut:self]; //Lets hide the sheet.
            
            if(result == true){
                if([_chkMakeBackup state]==NSOffState){
                    [fileManager removeFileAtPath:backupPath handler:nil];
                }
                NSRunAlertPanel(@"Finished!",@"The ROM image was patched sucessfully.",@"Okay",nil,nil);
            }
            else{
                NSRunAlertPanel(@"Patching problem.",[NSString stringWithCString:ups.error encoding:NSASCIIStringEncoding],@"Okay",nil,nil);
            }
        }
        else{
            NSRunAlertPanel(@"File error.",@"Unable to access the selected file.",@"Okay",nil,nil);
        }
    }
    else{
        NSRunAlertPanel(@"Patch not found",@"The patch file, mother3.ups does not exist.\nIt should've been included in the zip with this patcher.",@"Okay",nil,nil);
    }
}

- (IBAction)btnLostBrowseRom:(id)sender{
    NSOpenPanel *fbox = [NSOpenPanel openPanel];
    fbox.allowedFileTypes = @[@"gba",@"bin"];
    fbox.allowsOtherFileTypes = YES;
    [fbox beginSheetForDirectory:nil file:nil modalForWindow:_wndPatcher modalDelegate:self didEndSelector:@selector(openPanelDidEnd: returnCode: contextInfo:) contextInfo:nil];
    //Delegate is who handles the window code.
}

- (IBAction)btnLostBrowsePatch:(id)sender{
    NSOpenPanel *fbox = [NSOpenPanel openPanel];
    fbox.allowedFileTypes = @[@"ups"];
    fbox.allowsOtherFileTypes = NO;
    [fbox beginSheetForDirectory:nil file:nil modalForWindow:_wndPatcher modalDelegate:self didEndSelector:@selector(patchPanelDidEnd: returnCode: contextInfo:) contextInfo:nil];
    //Delegate is who handles the window code.
}

- (void)openPanelDidEnd:(NSOpenPanel *)panel returnCode:(int)returnCode  contextInfo:(void  *)contextInfo{
    if(returnCode == NSOKButton){
        NSString* selfile = [[panel filenames] objectAtIndex:0];
        [_txtRomPath setStringValue:selfile];
    }
}

- (void)patchPanelDidEnd:(NSOpenPanel *)panel returnCode:(int)returnCode  contextInfo:(void  *)contextInfo{
    if(returnCode == NSOKButton){
        NSString* selfile = [[panel filenames] objectAtIndex:0];
        [_txtPatchPath setStringValue:selfile];
    }
}

@end
