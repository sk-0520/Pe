import type { FC, ReactNode } from "react";
import type { PageKey } from "../../pages";
import {
	DevBranchPage,
	DevBuildPage,
	DevCiPage,
	DevIndexPage,
	DevPluginPage,
	DevPluginReferencePage,
	DevPluginTemplatePage,
	DevTableLargePage,
	DevTableMainPage,
	DevTableTemporaryPage,
} from "../../pages/dev";
import {
	HelpChangelogPage,
	HelpCpuPage,
	HelpIndexPage,
	HelpInstallUninstallDataPage,
	HelpLauncherCommandPage,
	HelpLauncherExtendsExecutePage,
	HelpLauncherPage,
	HelpLauncherToolbarPage,
	HelpNotePage,
	HelpNotifyAreaPage,
	HelpOthersAppSettingsPage,
	HelpOthersCommandLinePage,
	HelpOthersPage,
	HelpOthersPluginPage,
	HelpOthersProxyPage,
	HelpPrivacyPage,
	HelpSearchPage,
} from "../../pages/help";

import { Typography } from "@mui/material";
import type { PageProps } from "../../types/page";

interface PageContentProps extends PageProps {}

export class NotImplError extends Error {
	constructor(message: string) {
		super(message);
		this.name = this.constructor.name;
	}
}

const pageFactory: { [key in PageKey]: (props: PageProps) => ReactNode } = {
	"help.index": (props: PageProps) => <HelpIndexPage {...props} />,
	"help.install_uninstall_data": (props: PageProps) => (
		<HelpInstallUninstallDataPage {...props} />
	),
	"help.cpu": (props: PageProps) => <HelpCpuPage {...props} />,
	"help.privacy": (props: PageProps) => <HelpPrivacyPage {...props} />,
	"help.notify_area": (props: PageProps) => <HelpNotifyAreaPage {...props} />,
	"help.launcher": (props: PageProps) => <HelpLauncherPage {...props} />,
	"help.launcher_toolbar": (props: PageProps) => (
		<HelpLauncherToolbarPage {...props} />
	),
	"help.launcher_command": (props: PageProps) => (
		<HelpLauncherCommandPage {...props} />
	),
	"help.launcher_extends_execute": (props: PageProps) => (
		<HelpLauncherExtendsExecutePage {...props} />
	),
	"help.note": (props: PageProps) => <HelpNotePage {...props} />,
	"help.others": (props: PageProps) => <HelpOthersPage {...props} />,
	"help.others_appsettings": (props: PageProps) => (
		<HelpOthersAppSettingsPage {...props} />
	),
	"help.others_commandline": (props: PageProps) => (
		<HelpOthersCommandLinePage {...props} />
	),
	"help.others_proxy": (props: PageProps) => <HelpOthersProxyPage {...props} />,
	"help.others_plugin": (props: PageProps) => (
		<HelpOthersPluginPage {...props} />
	),
	"help.search": (props: PageProps) => <HelpSearchPage {...props} />,
	"help.changelog": (props: PageProps) => <HelpChangelogPage {...props} />,
	"dev.index": (props: PageProps) => <DevIndexPage {...props} />,
	"dev.build": (props: PageProps) => <DevBuildPage {...props} />,
	"dev.branch": (props: PageProps) => <DevBranchPage {...props} />,
	"dev.ci": (props: PageProps) => <DevCiPage {...props} />,
	"dev.table_main": (props: PageProps) => <DevTableMainPage {...props} />,
	"dev.table_large": (props: PageProps) => <DevTableLargePage {...props} />,
	"dev.table_temporary": (props: PageProps) => (
		<DevTableTemporaryPage {...props} />
	),
	"dev.plugin": (props: PageProps) => <DevPluginPage {...props} />,
	"dev.plugin_reference": (props: PageProps) => (
		<DevPluginReferencePage {...props} />
	),
	"dev.plugin_template": (props: PageProps) => (
		<DevPluginTemplatePage {...props} />
	),
} as const;

export const PageContent: FC<PageContentProps> = (props: PageContentProps) => {
	const { selectedPageKey } = props;

	const factory = pageFactory[selectedPageKey];
	try {
		const page = factory(props);
		return page;
	} catch (ex: unknown) {
		if (ex instanceof NotImplError) {
			return (
				<Typography color="error">
					Not Impl: <strong>{selectedPageKey}</strong>
				</Typography>
			);
		}
		throw ex;
	}
};
