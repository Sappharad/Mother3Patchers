#import <Cocoa/Cocoa.h>

@interface FoundPatchController : NSObject{
    IBOutlet id chkMakeBackup;
    IBOutlet id imgFancyLogo;
    IBOutlet id txtRomPath;
    IBOutlet id wndPatcher;
	IBOutlet id pnlPatching;
	IBOutlet id	barProgress;
}
- (void)showDialog;
- (IBAction)btnApply:(id)sender;
- (IBAction)btnBrowse:(id)sender;
- (void)openPanelDidEnd:(NSOpenPanel *)panel returnCode:(int)returnCode  contextInfo:(void  *)contextInfo; //Called automatically
@end
