import { NgModule } from '@angular/core';
import { PostContainerComponent } from './post-container/post-container';
import { ModuleWithProviders } from '@angular/compiler/src/core';
import { BrowserModule } from '@angular/platform-browser';

@NgModule({
	declarations: [ PostContainerComponent ],
	imports: [
		BrowserModule
	],
	exports: [ PostContainerComponent ]
})
export class ComponentsModule {
	public static forRoot(): ModuleWithProviders {
		return {
			ngModule: ComponentsModule
		};
	}
}
