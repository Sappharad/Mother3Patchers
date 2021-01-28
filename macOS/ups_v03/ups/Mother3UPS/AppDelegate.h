//
//  AppDelegate.h
//  Mother3UPS
//

#import <Cocoa/Cocoa.h>
#import "LostPatchController.h"
#import "FoundPatchController.h"

NS_ASSUME_NONNULL_BEGIN

@interface AppDelegate : NSObject <NSApplicationDelegate, NSWindowDelegate>
@property (assign) IBOutlet LostPatchController *lostController;
@property (assign) IBOutlet FoundPatchController *foundController;


@end

NS_ASSUME_NONNULL_END
