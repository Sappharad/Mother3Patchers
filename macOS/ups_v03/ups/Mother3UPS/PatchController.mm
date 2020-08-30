#import "PatchController.h"
#include "libups.hpp"

@implementation PatchController

- (IBAction)btnApply:(id)sender {
    NSFileManager *fileManager = [NSFileManager defaultManager];
	NSBundle *appResources = [NSBundle mainBundle]; //Get access to files inside the program
	NSString *romPath = [txtRomPath stringValue];
	NSString *backupPath = [romPath stringByAppendingString:@".bak"];
	NSString *patchPath = [appResources bundlePath];
	NSRange lastSlash = [patchPath rangeOfString:@"/" options:NSBackwardsSearch];
	patchPath = [patchPath substringToIndex:lastSlash.location];
	patchPath = [patchPath stringByAppendingString:@"/mother3.ups"];
	UPS ups; //UPS Patcher
	
	if([fileManager fileExistsAtPath:patchPath]){
		if([fileManager movePath:romPath toPath:backupPath handler:nil]){ //Make the backup by renaming the original file.
			[NSApp beginSheet:pnlPatching modalForWindow:wndPatcher modalDelegate:nil didEndSelector:nil contextInfo:nil]; //Make a sheet
			[barProgress setUsesThreadedAnimation:YES]; //Make sure it animates.
			[barProgress startAnimation:self];
			bool result = ups.apply([backupPath cString], [romPath cString], [patchPath cString]);
			[barProgress stopAnimation:self];
			[NSApp endSheet:pnlPatching]; //Tell the sheet we're done.
			[pnlPatching orderOut:self]; //Lets hide the sheet.
			
			if(result == true){
				if([chkMakeBackup state]==NSOffState){
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

- (IBAction)btnBrowse:(id)sender {
    NSOpenPanel *fbox = [NSOpenPanel openPanel];
	[fbox beginSheetForDirectory:nil file:nil modalForWindow:wndPatcher modalDelegate:self didEndSelector:@selector(openPanelDidEnd: returnCode: contextInfo:) contextInfo:nil];
	//Delegate is who handles the window code.
}

- (void)openPanelDidEnd:(NSOpenPanel *)panel returnCode:(int)returnCode  contextInfo:(void  *)contextInfo{
	if(returnCode == NSOKButton){
		NSString* selfile = [[panel filenames] objectAtIndex:0];
		[txtRomPath setStringValue:selfile];
	}
}

@end
