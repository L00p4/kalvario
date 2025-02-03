module.exports = (plop) => {
  // create your generators here
  plop.setGenerator('component', {
    description: 'application component',

    // inquirer prompts
    prompts: [
      {
        type: 'input',
        name: 'name',
        message: 'component name please?'
      }
    ],

    // actions to perform
    actions: [
      {
        type: 'add',
        path: '../src/ui/{{dashCase name}}/{{dashCase name}}.composition.tsx',
        templateFile: './templates/composition.tsx.hbs'
      },
      {
        type: 'add',
        path: '../src/ui/{{dashCase name}}/{{dashCase name}}.docs.mdx',
        templateFile: './templates/doc.mdx.hbs'
      },
      {
        type: 'add',
        path: '../src/ui/{{dashCase name}}/index.tsx',
        templateFile: './templates/index.tsx.hbs'
      },
      {
        type: 'add',
        path: '../src/ui/{{dashCase name}}/{{dashCase name}}.skeleton.tsx',
        templateFile: './templates/skeleton.tsx.hbs'
      },
      {
        type: 'add',
        path: '../src/ui/{{dashCase name}}/{{dashCase name}}.spec.tsx',
        templateFile: './templates/spec.tsx.hbs'
      },
      {
        type: 'add',
        path: '../src/ui/{{dashCase name}}/{{dashCase name}}.stories.tsx',
        templateFile: './templates/stories.tsx.hbs'
      },
      {
        type: 'add',
        path: '../src/ui/{{dashCase name}}/{{dashCase name}}.styles.ts',
        templateFile: './templates/styles.ts.hbs'
      }
    ]
  })
}
