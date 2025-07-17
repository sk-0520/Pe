import { create } from "zustand";
import { type PageKey, PageKeys } from "../pages";

const DefaultState: SideMenuState = {
	pageKey: PageKeys[0],
};

export interface SideMenuState {
	pageKey: PageKey;
}

export interface SideMenuAction {
	setPageKey: (pageKey: PageKey) => void;
}

export interface SideMenuStore extends SideMenuState, SideMenuAction {}

export const useSideMenuStore = create<SideMenuStore>((set, get) => {
	return {
		...DefaultState,

		setPageKey: (pageKey: PageKey) => {
			set({ pageKey: pageKey });
		},
	};
});
