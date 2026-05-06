import subprocess
import os
import sys

sys.path.insert(0, "vscons-build-utils/site_scons")

from build_utils import git_version, dotnet_run, vs_run, roslynator, get_scons_vs_option, setup_modinfo, setup_cake_build

vars = get_scons_vs_option()

env = Environment(variables=vars)
vars.Update(env)
vars.Save('.sconscache.py', env)
env.Help(vars.GenerateHelpText(env))
env["GIT_VERSION"] = git_version()



glideview_mod_info = setup_modinfo(env, "GlideView", False, True, "glideview", "Glide View", "Automaticaly switch to third person view when gliding but also when riding elk and sailing boat/raft")
glideview_cake = setup_cake_build(env, "CakeBuild", "GlideView", "Release")
glideview_sources = Glob("GlideView/*.cs")
env.Default([glideview_mod_info, glideview_cake])

fmt = env.Command(
    target=None,          # no build artifact
    source=[glideview_sources],
    action="clang-format -i $SOURCES"
)

env.Alias("format", fmt)
env.Alias("fmt", fmt)
#
#glideview_release = f"Release/glideview_{env["GIT_VERSION"]}.zip"
#
#def glideview_cake(target, source, env):
#    dotnet_run("./CakeBuild/CakeBuild.csproj", str(env["VINTAGE_STORY"]), str(env["DOTNET_VERS"]))
#
#env.Command(glideview_release, glideview_sources, glideview_cake)
#env.Clean(glideview_release, ['GlideView/bin', 'GlideView/obj', 'Release'])
#env.Default(glideview_release)
##smartcursor_install_release = env.InstallAs(target=f"{str(env["VINTAGE_STORY_DATA"])}/Mods/smartcursor.zip", source=smartcursor_release)
##env.Alias("install", smartcursor_install_release)
#env.Depends(glideview_release, [glideview_mod_info, glideview_cake])
