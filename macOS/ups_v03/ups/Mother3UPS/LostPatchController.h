//
//  LostPatchController.h
//  Mother3UPS
//
//  This version of the window is for newer versions of macOS where
//  App Translocation causes mother3.ups not to be located automatically.
//

#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

@interface LostPatchController : NSObject
@property (assign) IBOutlet NSButton *chkMakeBackup;
@property (assign) IBOutlet NSTextField *txtRomPath;
@property (assign) IBOutlet NSWindow *wndPatcher;
@property (assign) IBOutlet NSPanel *pnlPatching;
@property (assign) IBOutlet NSProgressIndicator *pbProgress;
@property (assign) IBOutlet NSTextField *txtPatchPath;

- (void)showDialog;
- (IBAction)btnLostApply:(id)sender;
- (IBAction)btnLostBrowseRom:(id)sender;
- (IBAction)btnLostBrowsePatch:(id)sender;
@end

NS_ASSUME_NONNULL_END
