import React from 'react';

class LoginForm extends React.Component {
    render() {
        return (
            <div className="w-full max-w-sm p-6 bg-white rounded-lg shadow-md">
                <a href="#" className="flex justify-center mb-6">
                    <img className="h-12" src={this.props.logoUrl} alt="Logo" />
                </a>
                <h1 className="text-2xl font-semibold text-center text-gray-700">{this.props.projectName.toUpperCase()}</h1>
                <form method="post" className="mt-6">
                    <div className="mb-4">
                        <div className="relative">
                            <input placeholder="USUARIO" name="username" autocomplete="off" className="w-full px-4 py-2 text-gray-700 bg-gray-200 border border-gray-300 rounded-md focus:border-blue-500 focus:outline-none focus:ring" />
                            <img className="absolute inset-y-0 right-3 h-5 w-5" src={this.props.userIcon} alt="User Icon" />
                        </div>
                    </div>
                    <div className="mb-4">
                        <div className="relative">
                            <input type="password" placeholder="CONTRASEÑA" name="password" className="w-full px-4 py-2 text-gray-700 bg-gray-200 border border-gray-300 rounded-md focus:border-blue-500 focus:outline-none focus:ring" />
                            <img className="absolute inset-y-0 right-3 h-5 w-5" src={this.props.passIcon} alt="Password Icon" />
                        </div>
                    </div>
                    <div className="mb-6">
                        <button className="w-full px-4 py-2 text-white bg-blue-600 rounded-md hover:bg-blue-700 focus:bg-blue-700">
                            INGRESAR
                        </button>
                    </div>
                    <div className="text-center">
                        <a className="text-sm text-blue-600 hover:underline" href="/Account/ForgotPassword">¿Olvidaste tu contraseña?</a>
                    </div>
                </form>
            </div>
        );
    }
}

export default LoginForm;
