//
//  AppDelegate.m
//  Mother3UPS
//

#import "AppDelegate.h"

@implementation AppDelegate

-(void)applicationDidFinishLaunching:(NSNotification *)notification{
    NSString* patchPath = [NSBundle.mainBundle.bundlePath stringByDeletingLastPathComponent];
    patchPath = [patchPath stringByAppendingString:@"/mother3.ups"];
    if([[NSFileManager defaultManager] fileExistsAtPath:patchPath]){
        [_foundController showDialog];
    }
    else{
        [_lostController showDialog];
    }
}

-(BOOL)applicationShouldTerminateAfterLastWindowClosed:(NSApplication *)sender{
    return YES;
}

@end
